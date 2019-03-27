using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiCommon.HealthCheck
{
    public class SimpleReadinessHealthCheck : IHealthCheck
    {
        public SimpleReadinessHealthCheck()
        {
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Readiness check

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
