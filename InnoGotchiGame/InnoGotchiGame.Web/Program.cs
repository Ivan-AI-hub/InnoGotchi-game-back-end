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
using InnoGotchiGame.Web.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
					  policy =>
					  {
						  policy.WithOrigins("https://localhost:7192",
											 "http://localhost:5058");
					  });
});


builder.Services.AddControllers();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InnoGotchiGameContext>(options => options.UseSqlServer(connection));

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
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
