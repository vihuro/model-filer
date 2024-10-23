using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils.Filters;
using ModelFilter.Persistence.Context;
using ModelFilter.Persistence.Repository;
using ModelFilter.Persistence.Utils;

namespace ModelFilter.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistenceApp(this IServiceCollection services,
                                                   IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("postgres");
            services.AddDbContext<AppDbContext>(op => op.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFilterDynamic, FilterDynamic>();
            services.AddScoped<IFilterInterpreterFactory, FilterInterpreterFactory>();
            services.AddScoped<ICustomNotification, NotificationService>();

        }
    }
}
