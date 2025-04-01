using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseService.Extensions
{
    /// <summary>
    /// Добавить сервис работы с данными.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BotDbContext>(options =>
            {
                options.UseSqlite(connectionString);
                options.LogTo(Console.WriteLine);
            });
            services.AddTransient<IDataService, DataService>();

            return services;
        }
    }
}