using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Shared.Services;
using System.Data;
using System.Text;
using WebAPI.Features.Login.Models;
using UserModel = WebAPI.Features.User.Models.User;
using WebAPI.Infrastructure.Data;
using WebAPI.Infrastructure.Repository;
using WebAPI.Middlewares;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = environment
});
var configuration = builder.Configuration;

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80); // Configura para escutar na porta 80
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration["ConnectionStrings:PostgreConnection"]);
    options.AddInterceptors(new SlowQueryInterceptor());
    options
        .LogTo(Console.WriteLine, LogLevel.Warning) // Exibe apenas logs de Warning ou superior
        .EnableSensitiveDataLogging(false);
});

// Add Identity
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgreConnection")));

//Inject Services
builder.Services.AddSingleton<IEmailService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var emailSettings = configuration.GetSection("EmailSettings");
    return new EmailService(
        emailSettings["SmtpHost"],
        int.Parse(emailSettings["SmtpPort"]),
        emailSettings["Username"],
        emailSettings["Password"],
        emailSettings["FromEmail"],
        emailSettings["FromName"]
    );
});

//Inject Repository
builder.Services.AddScoped<IEntubaRepository, EntubaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
builder.Services.AddScoped<IPasswordHasher<LoginModel>, PasswordHasher<LoginModel>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure JWT Authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sample API",
        Version = "v1",
        Description = "API base da FL-SoftwareHouse",
        Contact = new OpenApiContact
        {
            Name = "FL Softwarehouse",
            Email = "flsoftwarehouse@gmail.com",
        }
    });

    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtAuthenticationMiddleware>();


        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chave do Saber API v1");
            c.RoutePrefix = "swagger"; // Para abrir o Swagger diretamente na raiz do app
        });


app.MapControllers();
app.Run();