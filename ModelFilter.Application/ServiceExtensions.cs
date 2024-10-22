using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ModelFilter.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplicationApp(this IServiceCollection services)
        {
            services.AddMediatR(md =>
                        md.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
