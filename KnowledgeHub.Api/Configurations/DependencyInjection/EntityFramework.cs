using KnowledgeHub.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class EntityFramework
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
