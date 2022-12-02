using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using InnoGotchiGame.Application.Validators;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using InnoGotchiGame.Web;
using Microsoft.IdentityModel.Tokens;
using InnoGotchiGame.Web.Mapping;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InnoGotchiGameContext>(options => options.UseSqlServer(connection));

var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile>() { new AssemblyMappingProfile(), new WebMappingProfile() }));

builder.Services.AddTransient<IMapper>(x => new Mapper(config));

builder.Services.AddTransient<AbstractValidator<User>, UserValidator>();
builder.Services.AddTransient<IRepository<User>, UserRepository>();
builder.Services.AddTransient<IRepository<ColaborationRequest>, ColaborationRequestRepository>();

builder.Services.AddTransient<UserManager>();
builder.Services.AddTransient<ColaborationRequestManager>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options =>
					{
						options.RequireHttpsMetadata = false;
						options.TokenValidationParameters = new TokenValidationParameters
						{
							// ��������, ����� �� �������������� �������� ��� ��������� ������
							ValidateIssuer = true,
							// ������, �������������� ��������
							ValidIssuer = AuthOptions.ISSUER,

							// ����� �� �������������� ����������� ������
							ValidateAudience = true,
							// ��������� ����������� ������
							ValidAudience = AuthOptions.AUDIENCE,
							// ����� �� �������������� ����� �������������
							ValidateLifetime = true,

							// ��������� ����� ������������
							IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
							// ��������� ����� ������������
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

app.UseAuthorization();

app.MapControllers();

app.Run();
