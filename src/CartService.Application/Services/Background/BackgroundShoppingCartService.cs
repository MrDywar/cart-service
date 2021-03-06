﻿using CartService.Application.DTO.Reports;
using CartService.Application.Repositories;
using CartService.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using Polly.Registry;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CartService.Application.Services.Background
{
    public class BackgroundShoppingCartService : IBackgroundShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IReportSaver _reportSaver;

        private readonly string _onDeleteWebhookUrl;
        private readonly AsyncPolicyWrap<HttpResponseMessage> _webhookApiPolicy;

        public BackgroundShoppingCartService(
            IUnitOfWork unitOfWork,
            IHttpClientFactory clientFactory,
            IReportSaver reportSaver,
            IConfiguration configuration,
            IReadOnlyPolicyRegistry<string> registry)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = clientFactory;
            _reportSaver = reportSaver;

            _onDeleteWebhookUrl = configuration.GetValue<string>("ShoppingCartOnDeleteWebhookUrl");
            _webhookApiPolicy = registry.Get<AsyncPolicyWrap<HttpResponseMessage>>("webhookApiPolicy");
        }

        public async Task GenerateReport()
        {
            var cartsReport = await _unitOfWork.RunInTrunsaction(async (con, tran) =>
            {
                var sqlQuery = @"
                    -- total carts
                    SELECT COUNT (*)
                    FROM ShoppingCart

                    -- total carts with bonuses
                    SELECT COUNT (DISTINCT C.Id)
                    FROM ShoppingCart AS C
                    JOIN ShoppingCartItem AS CI ON CI.ShoppingCartId = C.Id
                    JOIN Product AS P ON P.Id = CI.ProductId AND P.ForBonusPoints = 1

                    -- total expired in 10 days
                    SELECT COUNT (*)
                    FROM ShoppingCart
                    WHERE CAST(LatestUpdatedOn AS DATE) BETWEEN @StartDay10 AND @EndDay10

                    -- total expired in 20 days
                    SELECT COUNT (*)
                    FROM ShoppingCart
                    WHERE CAST(LatestUpdatedOn AS DATE) BETWEEN @StartDay20 AND @EndDay20

                    -- total expired in 30 days
                    SELECT COUNT (*)
                    FROM ShoppingCart
                    WHERE CAST(LatestUpdatedOn AS DATE) BETWEEN @StartDay30 AND @EndDay30

                    -- average check
                    SELECT AVG (CARTS_STAT.CART_SUM)
                    FROM (
                      SELECT C.Id AS CART_Id, SUM (CI.Cost * CI.Quantity) AS CART_SUM
                      FROM ShoppingCart AS C
                      JOIN ShoppingCartItem AS CI ON CI.ShoppingCartId = C.Id
                      GROUP BY C.Id
                    ) AS CARTS_STAT
                ";

                Func<int, string> getDateFunc = (int days) => DateTime.UtcNow.AddDays(days).ToString("yyyy-MM-dd");

                var args = new
                {
                    StartDay10 = getDateFunc(-30), EndDay10 = getDateFunc(-20),
                    StartDay20 = getDateFunc(-30), EndDay20 = getDateFunc(-10),
                    StartDay30 = getDateFunc(-30), EndDay30 = getDateFunc(0),
                };

                var result = new ShoppingCartsReportDTO();

                using (var multi = await con.QueryMultipleAsync(sqlQuery, args, tran))
                {
                    result.TotalCarts = multi.Read<int>().Single();
                    result.TotalCartsWithBonuses = multi.Read<int>().Single();
                    result.TotalCartsExpiredIn10Days = multi.Read<int>().Single();
                    result.TotalCartsExpiredIn20Days = multi.Read<int>().Single();
                    result.TotalCartsExpiredIn30Days = multi.Read<int>().Single();
                    result.AverageCheck = multi.Read<decimal>().Single();
                }

                return result;
            });

            var fileName = $"shoppingCartReport_{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
            await _reportSaver.SaveShoppingCartReport(fileName, cartsReport);
        }

        public async Task DeleteExpiredShoppingCarts()
        {
            var olderThanUtc = DateTime.UtcNow.AddDays(-30);

            var expiredCarts = new List<ShoppingCart>();

            await _unitOfWork.RunInTrunsaction(async (con, tran) =>
            {
                var sqlSelectQuery = @"
                    SELECT TOP 100 *
                    FROM ShoppingCart
                    WHERE LatestUpdatedOn <= @olderThanUtc";

                expiredCarts.AddRange(await con.QueryAsync<ShoppingCart>(sqlSelectQuery, new { olderThanUtc }, tran));

                var expiredCartIds = expiredCarts.Select(x => x.Id).ToArray();

                var sqlDeleteQuery = @"
                    DELETE
                    FROM ShoppingCart
                    WHERE Id IN @Ids";

                return await con.ExecuteAsync(sqlDeleteQuery, new { Ids = expiredCartIds }, tran);
            });

            // ignore errors, or use repeatable jobs (persist storage)
            foreach (var cart in expiredCarts)
            {
                var content = new StringContent(JsonSerializer.Serialize(cart), Encoding.Default, "application/json");

                var client = _httpClientFactory.CreateClient();

                await _webhookApiPolicy.ExecuteAndCaptureAsync(async ct =>
                    await client.PostAsync(_onDeleteWebhookUrl, content, ct),
                    CancellationToken.None);
            }
        }
    }
}
