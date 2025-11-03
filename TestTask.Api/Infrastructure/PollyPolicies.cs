using Polly;
using Polly.Extensions.Http;

namespace TestTask.Infrastructure;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                3, i => TimeSpan.FromSeconds(Math.Pow(2, i))
            );
}

