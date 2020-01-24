
using Polly;
using Polly.Wrap;
using System;
using System.Net.Http;

namespace CartService.Infrastructure.Policies
{
    public class WebhookApiPolicy
    {
        public static AsyncPolicyWrap<HttpResponseMessage> Create()
        {
            var apiTimeoutMs = 1000;
            var exceptionsCountToBreak = 4;
            var breakDurationSec = 30;

            var timeout = Policy
                .TimeoutAsync(
                    TimeSpan.FromMilliseconds(apiTimeoutMs),
                    Polly.Timeout.TimeoutStrategy.Optimistic
                );

            var circuitBreaker = Policy<HttpResponseMessage>
                .Handle<Exception>() //TODO: ignore user input errors, or catch well known external server errors (403,500...).
                .CircuitBreakerAsync(
                    exceptionsCountToBreak,
                    TimeSpan.FromSeconds(breakDurationSec)
                );

            return circuitBreaker.WrapAsync(timeout);
        }
    }
}
