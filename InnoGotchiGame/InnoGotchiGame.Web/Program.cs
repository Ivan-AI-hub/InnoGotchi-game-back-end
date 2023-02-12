using FluentValidation;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Mappings;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Web;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Mapping;
using InnoGotchiGame.Web.Middleware;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(builder.Configuration, MyAllowSpecificOrigins);
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AssemblyMappingProfile), typeof(WebMappingProfile));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddTransient<AbstractValidator<User>, UserValidator>();
builder.Services.AddTransient<UserManager>();

builder.Services.AddTransient<AbstractValidator<Picture>, PictureValidator>();
builder.Services.AddTransient<PictureManager>();

builder.Services.AddTransient<ColaborationRequestManager>();

builder.Services.AddTransient<AbstractValidator<Pet>, PetValidator>();
builder.Services.AddTransient<PetManager>();

builder.Services.AddTransient<AbstractValidator<PetFarm>, PetFarmValidator>();
builder.Services.AddTransient<PetFarmManager>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<BasePetViewInitializer>();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.UseAuthentication();

app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.MapControllers();

app.Run();
