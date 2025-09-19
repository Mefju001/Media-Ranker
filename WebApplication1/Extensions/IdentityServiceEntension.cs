using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApplication1.Extensions
{
    public static class IdentityServiceEntension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var key = config["Jwt:Key"];

            // Validate that the key exists before using it
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is missing from the configuration. Please add it to appsettings.json under 'Jwt' section.");
            }
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Issuer"],
                        ValidAudience = config["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key)
                                )
                    };
                });
            return services;
        }
    }
}
