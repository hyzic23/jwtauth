using System.Text;
using FluentValidation;
using jwtauth.api.Config;
using jwtauth.api.Dtos;
using jwtauth.api.Validators;
using jwtauth.dataaccess.Data;
using jwtauth.dataaccess.IService;
using jwtauth.dataaccess.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<Redis>(builder.Configuration.GetSection("Redis"));
builder.Services.AddDbContext<DatabaseContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("dbConnect")));
builder.Services.AddStackExchangeRedisCache(o => { o.Configuration = configuration["Redis:RedisCacheUrl"]; });
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IUserInfoService, UserInfoService>();
//builder.Services.AddTransient<ICacheService, CacheService>();
//builder.Services.AddTransient<IGenericService, GenericService>();
builder.Services.AddSingleton<IAppSettings, AppSettings>();
builder.Services.AddScoped<IValidator<UserInfoDto>, UserInfoDtoValidator>();
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Registering Redis
builder.Services.AddStackExchangeRedisCache(config =>
{
    config.Configuration = builder.Configuration["Redis:RedisCacheUrl"];
});

//Configuring logging
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

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
