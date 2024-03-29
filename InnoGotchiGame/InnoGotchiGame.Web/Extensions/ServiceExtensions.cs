﻿using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence;
using InnoGotchiGame.Persistence.Managers;
using InnoGotchiGame.Persistence.Repositories;
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
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration, string allowedOriginsSectionName, string policyName = "CorsPolicy")
        {
            IEnumerable<string> allowedOrigins = configuration.GetSection(allowedOriginsSectionName)
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

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration, string connectionStringSectionName)
        {
            var connection = configuration.GetConnectionString(connectionStringSectionName);
            services.AddDbContext<InnoGotchiGameContext>(options =>
                                options.UseSqlServer(connection,
                                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IColaborationRequestRepository, ColaborationRequestRepository>();
            services.AddScoped<IPetFarmRepository, PetFarmRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void ConfigureBLLManagers(this IServiceCollection services)
        {
            services.AddScoped<UserManager>();
            services.AddScoped<PictureManager>();
            services.AddScoped<ColaborationRequestManager>();
            services.AddScoped<PetManager>();
            services.AddScoped<PetFarmManager>();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration,
                                        string jwtSettingsSection, string issuerSection, string audienceSection, string keySection)
        {
            var jwtSettings = configuration.GetSection(jwtSettingsSection);
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
                    ValidIssuer = jwtSettings.GetSection(issuerSection).Value,
                    ValidAudience = jwtSettings.GetSection(audienceSection).Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection(keySection).Value))
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
