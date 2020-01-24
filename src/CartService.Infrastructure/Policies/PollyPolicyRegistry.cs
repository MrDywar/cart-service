using Polly.Registry;

namespace CartService.Infrastructure.Policies
{
    public class PollyPolicyRegistry
    {
        public static PolicyRegistry Create()
        {
            return new PolicyRegistry()
            {
                { "webhookApiPolicy", WebhookApiPolicy.Create() }
            };
        }
    }
}
