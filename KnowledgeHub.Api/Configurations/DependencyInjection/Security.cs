using KnowledgeHub.Application.Abstrations.Security;
using KnowledgeHub.Infra.Security;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class Security
    {
        public static IServiceCollection AddPasswordHasher(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
            return services;
        }
    }
}
