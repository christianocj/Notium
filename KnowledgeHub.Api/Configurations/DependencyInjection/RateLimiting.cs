using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.RateLimiting;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class RateLimiting
    {
        public static IServiceCollection AddMyRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueLimit = 0;
                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            // Rate Limiting para GetAll em notes. Apenas testes
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("notes-policy", context =>
                {
                    var userId = context.User.FindFirst(
                        JwtRegisteredClaimNames.Sub)?.Value;

                    userId ??= "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: userId,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }
    }
}
