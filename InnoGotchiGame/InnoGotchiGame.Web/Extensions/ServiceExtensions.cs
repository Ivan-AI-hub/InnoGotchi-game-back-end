﻿using InnoGotchiGame.Persistence;
using InnoGotchiGame.Persistence.Managers;
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

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureResponseCaching(this IServiceCollection services)
        { 
            services.AddResponseCaching(); 
        }

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders();
            services.AddHttpContextAccessor();
        }
    }
}
