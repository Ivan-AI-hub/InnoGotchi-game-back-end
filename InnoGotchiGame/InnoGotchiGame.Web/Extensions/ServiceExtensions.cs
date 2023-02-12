using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Persistence;
using InnoGotchiGame.Persistence.Managers;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

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

        public static void ConfigureBLLManagers(this IServiceCollection services)
        {
            services.AddScoped<UserManager>();
            services.AddScoped<PictureManager>();
            services.AddScoped<ColaborationRequestManager>();
            services.AddScoped<PetManager>();
            services.AddScoped<PetFarmManager>(); ;
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

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("issuerSigningKey").Value))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                           Name = "Bearer",
                        },
                        new List<string>()
                    }
                });

            });
        }
    }
}
