using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace QuizRoyaleAPI.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static void AddJWT(this IServiceCollection services, string key)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        RequireExpirationTime = false,
                        ValidIssuer = key
                    };
                });
        }
    }
}
