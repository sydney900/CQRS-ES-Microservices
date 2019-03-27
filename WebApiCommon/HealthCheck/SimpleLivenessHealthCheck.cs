using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiCommon.HealthCheck
{
    public class SimpleLivenessHealthCheck : IHealthCheck
    {
        public SimpleLivenessHealthCheck()
        {
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Liveness check

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
