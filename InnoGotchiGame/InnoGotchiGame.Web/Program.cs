using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Mappings;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Repositories;
using InnoGotchiGame.Web;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Mapping;
using InnoGotchiGame.Web.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(builder.Configuration, MyAllowSpecificOrigins);
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);


var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile>() { new AssemblyMappingProfile(), new WebMappingProfile() }));

builder.Services.AddTransient<IMapper>(x => new Mapper(config));

builder.Services.AddTransient<AbstractValidator<User>, UserValidator>();
builder.Services.AddTransient<IRepository<User>, UserRepository>();
builder.Services.AddTransient<UserManager>();

builder.Services.AddTransient<AbstractValidator<Picture>, PictureValidator>();
builder.Services.AddTransient<IRepository<Picture>, PictureRepository>();
builder.Services.AddTransient<PictureManager>();

builder.Services.AddTransient<IRepository<ColaborationRequest>, ColaborationRequestRepository>();
builder.Services.AddTransient<ColaborationRequestManager>();

builder.Services.AddTransient<IRepository<Pet>, PetRepository>();
builder.Services.AddTransient<AbstractValidator<Pet>, PetValidator>();
builder.Services.AddTransient<PetManager>();

builder.Services.AddTransient<IRepository<PetFarm>, PetFarmRepository>();
builder.Services.AddTransient<AbstractValidator<PetFarm>, PetFarmValidator>();
builder.Services.AddTransient<PetFarmManager>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidIssuer = AuthOptions.ISSUER,

            ValidateAudience = true,

            ValidAudience = AuthOptions.AUDIENCE,

            ValidateLifetime = true,

            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

            ValidateIssuerSigningKey = true,
        };
    });
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<BasePetViewInitializer>();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
