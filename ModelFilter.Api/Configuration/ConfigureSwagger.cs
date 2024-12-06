using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ModelFilter.Api.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigureSwagger
    {
        /// <summary>
        /// Swagger configuration
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwaggerApp(this IServiceCollection services)
        {
            var appVersion = typeof(Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
            services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Filter", Version = appVersion });

                g.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                g.IncludeXmlComments(xmlPath);

                g.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

            });

        }
    }
}
