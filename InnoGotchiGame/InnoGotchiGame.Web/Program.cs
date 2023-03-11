using FluentValidation;
using InnoGotchiGame.Application.Mappings;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Initializers.Models;
using InnoGotchiGame.Web.Mapping;
using InnoGotchiGame.Web.Middleware;
using NLog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors(builder.Configuration, "AllowedSpecificOrigins", MyAllowSpecificOrigins);
builder.Services.ConfigureSqlContext(builder.Configuration, "DefaultConnection");

builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureBLLManagers();

builder.Services.ConfigureSwagger();

builder.Services.ConfigureJWT(builder.Configuration, "JwtSettings", "validIssuer", "validAudience", "issuerSigningKey");
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddAutoMapper(typeof(AssemblyMappingProfile), typeof(WebMappingProfile));

builder.Services.AddValidatorsFromAssemblyContaining<PetValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Configuration.AddJsonFile("BasePetViewConfig.json");
builder.Services.Configure<BasePetViewInitializeModel>(builder.Configuration);

var app = builder.Build();
app.InitializePetView();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
