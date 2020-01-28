
using Polly;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace CartService.Infrastructure.Policies
{
    public class WebhookApiPolicy
    {
        public static AsyncPolicyWrap<HttpResponseMessage> Create()
        {
            var apiTimeoutMs = 500;
            var retryCount = 3;
            var exceptionsCountToBreak = 2;
            var breakDurationSec = 30;

            var timeout = Policy
                .TimeoutAsync(
                    TimeSpan.FromMilliseconds(apiTimeoutMs),
                    TimeoutStrategy.Optimistic
                );

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
               HttpStatusCode.RequestTimeout,
               HttpStatusCode.InternalServerError,
               HttpStatusCode.BadGateway,
               HttpStatusCode.ServiceUnavailable,
               HttpStatusCode.GatewayTimeout
            };

            var retry = Policy
                .Handle<TimeoutRejectedException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(retryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(3, retryAttempt))
                );

            var circuitBreaker = Policy<HttpResponseMessage>
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsCountToBreak,
                    TimeSpan.FromSeconds(breakDurationSec)
                );

            return circuitBreaker.WrapAsync(retry).WrapAsync(timeout);
        }
    }
}
