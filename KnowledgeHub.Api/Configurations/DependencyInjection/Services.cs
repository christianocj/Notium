using KnowledgeHub.Application.Abstrations.Services;
using KnowledgeHub.Application.Services;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<INoteService, NoteService>();

            return services;
        }
    }
}
