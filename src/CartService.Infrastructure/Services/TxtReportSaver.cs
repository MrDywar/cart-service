using CartService.Application.DTO.Reports;
using CartService.Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infrastructure.Services
{
    public class TxtReportSaver : IReportSaver
    {
        private readonly string _dirPath;

        public TxtReportSaver(IConfiguration configuration)
        {
            _dirPath = configuration.GetValue<string>("ReportsDirectoryPath");
        }

        public async Task SaveShoppingCartReport(string fileName, ShoppingCartsReportDTO data)
        {
            var fullName = Path.Combine(_dirPath, $"{fileName}.txt");
            var fileContent = ConvertShoppingCartsReportDTO(data);

            using (var stream = new FileStream(
                fullName, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
            {
                var bytes = Encoding.UTF8.GetBytes(fileContent);

                //await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private string ConvertShoppingCartsReportDTO(ShoppingCartsReportDTO data)
        {
            var result = new StringBuilder();

            result.AppendLine($"Total carts: {data.TotalCarts}");
            result.AppendLine($"Total carts with bonuses: {data.TotalCartsWithBonuses}");
            result.AppendLine($"Total carts expired in 10 days: {data.TotalCartsExpiredIn10Days}");
            result.AppendLine($"Total carts expired in 20 days: {data.TotalCartsExpiredIn20Days}");
            result.AppendLine($"Total carts expired in 30 days: {data.TotalCartsExpiredIn30Days}");
            result.AppendLine($"Average check: {data.AverageCheck}");

            return result.ToString();
        }
    }
}
