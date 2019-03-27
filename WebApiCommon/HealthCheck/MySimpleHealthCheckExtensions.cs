using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCommon.HealthCheck
{
    public static class MySimpleHealthCheckExtensions
    {
        public static IHealthChecksBuilder AddMySimpleHealthCheck(this IHealthChecksBuilder build)
        {
            return build.AddCheck<SimpleLivenessHealthCheck>("liveness", HealthStatus.Unhealthy, new string[] { "liveness" })
            .AddCheck<SimpleReadinessHealthCheck>("readiness", HealthStatus.Unhealthy, new string[] { "readiness" });
        }

        public static IApplicationBuilder UseMySimpleHealthCheck(this IApplicationBuilder app)
        {
            return app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                Predicate = r => r.Tags.Contains("liveness")
            })
            .UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = r => r.Tags.Contains("readiness")
            });

        }
    }
}
