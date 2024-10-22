using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ModelFilter.Domain.Utils.Filters;
using System.Reflection;

namespace ModelFilter.Api.Configuration
{
    public static class ConfigureSwagger
    {
        public static void ConfigureSwaggerApp(this IServiceCollection services)
        {
            services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Filter", Version = "V1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Ajuste conforme necessário
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                g.IncludeXmlComments(xmlPath);

            });


        }
    }
}
