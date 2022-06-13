using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace QuizRoyaleAPI.Extensions
{
    /// <summary>
    /// AuthenticationConfiguration, deze statiche klasse heeft een methode om alle authenticatie instellingen te setten met betrekking tot tokens
    /// </summary>
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// Configureerd alle instellingen voor de token authenticatie
        /// </summary>
        /// <param name="services">Alle extensions</param>
        /// <param name="key">De secret key voor token generatie en validatie</param>
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
