using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelFilter.Persistence.Context;

namespace ModelFilter.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistenceApp(this IServiceCollection services,
                                                   IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("postgres");
            services.AddDbContext<AppDbContext>(op => op.UseNpgsql(connectionString));


        }
    }
}
