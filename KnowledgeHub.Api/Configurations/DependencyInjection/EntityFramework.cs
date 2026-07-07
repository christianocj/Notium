using KnowledgeHub.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class EntityFramework
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null)
                    ));
            return services;
        }
    }
}
