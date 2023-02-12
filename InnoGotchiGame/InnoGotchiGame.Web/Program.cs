using FluentValidation;
using FluentValidation.AspNetCore;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Mappings;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Mapping;
using InnoGotchiGame.Web.Middleware;
using NLog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(builder.Configuration, MyAllowSpecificOrigins);
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureBLLManagers();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AssemblyMappingProfile), typeof(WebMappingProfile));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddValidatorsFromAssemblyContaining<PetValidator>();

builder.Services.AddEndpointsApiExplorer();
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
