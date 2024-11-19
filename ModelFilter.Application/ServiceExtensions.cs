using Microsoft.Extensions.DependencyInjection;
using ModelFilter.Application.Interface;
using ModelFilter.Application.UseCases.Token;
using System.Reflection;

namespace ModelFilter.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationApp(this IServiceCollection services)
        {
            services.AddMediatR(md =>
                        md.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IJwtService, GenerateTokenHandle>();
        }
    }
}
