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
                "ShoppingCartGenerateReport",
                x => x.GenerateReport(),
                Cron.Daily); // Cron.MinuteInterval(1)

            RecurringJob.AddOrUpdate<IBackgroundShoppingCartService>(
                "DeleteExpiredShoppingCarts",
                x => x.DeleteExpiredShoppingCarts(),
                Cron.Daily(2)); // Cron.MinuteInterval(2)
        }
    }
}
