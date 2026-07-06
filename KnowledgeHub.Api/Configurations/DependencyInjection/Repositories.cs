using KnowledgeHub.Domain.Abstrations.Repository;
using KnowledgeHub.Infra.Repositories;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class Repositories
    {
        public static IServiceCollection Addrepositories(this IServiceCollection services) 
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();

            return services;
        }
    }
}
