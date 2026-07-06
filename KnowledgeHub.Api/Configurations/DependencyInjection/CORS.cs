namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class CORS
    {
        public static IServiceCollection AddMyCores(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazor", policy =>
                    policy.WithOrigins("https://localhost:7100", "http://localhost:5100") 
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });
            return services;
        }
    }
}
