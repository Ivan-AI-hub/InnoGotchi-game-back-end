using InnoGotchiGame.Persistence;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InnoGotchiGame.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, string policyName = "CorsPolicy")
        {
            IEnumerable<string> allowedOrigins = configuration.GetSection("AllowedSpecificOrigins")
                                                              .AsEnumerable()
                                                              .Select(x => x.Value)
                                                              .Where(x => x != null)!;
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, policy =>
                    policy.WithOrigins(allowedOrigins.ToArray())
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());

            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<InnoGotchiGameContext>(options => 
                                options.UseSqlServer(connection, 
                                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
        }
    }
}
