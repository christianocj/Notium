using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KnowledgeHub.Api.Configurations.DependencyInjection
{
    public static class Auth
    {
        public static IServiceCollection AddMyAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"]!;
            var jwtIssuer = configuration["Jwt:Issuer"]!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtIssuer,
                            ValidAudience = jwtIssuer,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtKey))
                        };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
