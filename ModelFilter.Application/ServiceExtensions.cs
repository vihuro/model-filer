using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelFilter.Application.Interface;
using ModelFilter.Application.UseCases.Token;
using ModelFilter.Application.Utils;
using System.Reflection;

namespace ModelFilter.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationApp(this IServiceCollection services,
                                                  IConfiguration configuration)
        {
            services.AddMediatR(md =>
                        md.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IJwtService, GenerateTokenHandle>();

            services.Configure<TokenSettings>(x =>
            {
                x.HoursExpiresRefreshToken = Convert.ToInt32(configuration.GetSection("tokenSettings:HoursExpiresRefreshToken").Value);
                x.HoursExpiresAccessToken = Convert.ToInt32(configuration.GetSection("tokenSettings:HoursExpiresAccessToken").Value);
                x.Key = configuration.GetSection("tokenSettings:Key").Value;

            });
        }
    }
}
