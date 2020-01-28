using CartService.Application.Services.Background;
using Hangfire;
using System;

namespace CartService.Infrastructure.Background
{
    public class HangFireJobScheduler
    {
        public static void Configure()
        {
            RecurringJob.AddOrUpdate<IBackgroundShoppingCartService>(
                "BackgroundShoppingCartService_GenerateReport",
                x => x.GenerateReport(),
                Cron.Daily); // Cron.MinuteInterval(1)

            RecurringJob.AddOrUpdate<IBackgroundShoppingCartService>(
                "BackgroundShoppingCartService_DeleteExpiredShoppingCarts",
                x => x.DeleteExpiredShoppingCarts(),
                $"*/25 * * * *"); // Cron.MinuteInterval(25)
        }
    }
}
