namespace QuizRoyaleAPI.Extensions
{
    /// <summary>
    /// SwaggerConfiguration, Deze statiche klasse heeft een methode om swagger te regelen
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Deze mehode set alle instellingen met betrekking tot Swagger op voor de documentatie
        /// </summary>
        /// <param name="services">Alle extensions</param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Om toegang te krijgen tot de API, moet een geldig JWT-token worden verschaft."
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                }
                            },
                            new string[] { }
                    }
                });
            });
        }
    }
}
