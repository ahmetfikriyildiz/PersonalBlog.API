using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalBlog.API.Data;
using PersonalBlog.API.Middlewares;
using PersonalBlog.API.Repositories.Implementations;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Implementations;
using PersonalBlog.API.Services.Interfaces;
using PersonalBlog.API.Settings;
using System.Text;
using Microsoft.Extensions.Options;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // JSON serialization'ı camelCase'e çevir
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// FluentValidation Configuration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// CORS Configuration - Environment variable öncelikli
// Öncelik sırası: Environment Variable > appsettings.{Environment}.json > appsettings.json > Default Development origins

// Environment variable'dan CORS origins'leri oku (virgülle ayrılmış)
var corsOriginsEnv = builder.Configuration["CORS__AllowedOrigins"];
var corsOrigins = !string.IsNullOrWhiteSpace(corsOriginsEnv)
    ? corsOriginsEnv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    : Array.Empty<string>();

// Configuration'dan CORS ayarlarını oku
var corsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>() ?? new CorsSettings();

// Environment variable'dan gelen origins varsa, onları kullan
if (corsOrigins.Length > 0)
{
    corsSettings.AllowedOrigins = corsOrigins;
}

// Development ortamında ve origins boşsa, default development origins kullan
if (builder.Environment.IsDevelopment() && corsSettings.AllowedOrigins.Length == 0)
{
    corsSettings.AllowedOrigins = new[]
    {
        "http://localhost:3000",
        "http://localhost:5173",
        "https://localhost:7281",
        "http://localhost:5098"
    };
}

// Production'da origins zorunlu
if (builder.Environment.IsProduction() && corsSettings.AllowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "CORS AllowedOrigins is required in Production. " +
        "Please set CORS__AllowedOrigins environment variable (comma-separated) " +
        "or configure in appsettings.Production.json. " +
        "Example: CORS__AllowedOrigins=https://yourdomain.com,https://www.yourdomain.com");
}

// CORS Settings'i DI container'a kaydet
builder.Services.Configure<CorsSettings>(options =>
{
    options.AllowedOrigins = corsSettings.AllowedOrigins;
    options.AllowCredentials = corsSettings.AllowCredentials;
});

// CORS Policy oluştur
builder.Services.AddCors(options =>
{
    options.AddPolicy("RestrictedCors", policy =>
    {
        if (corsSettings.AllowedOrigins.Length > 0)
        {
            policy.WithOrigins(corsSettings.AllowedOrigins)
                  .AllowAnyMethod() // GET, POST, PUT, PATCH, DELETE, OPTIONS
                  .AllowAnyHeader() // Content-Type, Authorization, Accept, etc.
                  .AllowCredentials(); // JWT token göndermek için gerekli
        }
        else
        {
            // Fallback: Eğer hiç origin yoksa (sadece development'ta olabilir)
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// CORS Configuration logging (app build edildikten sonra loglayacağız)

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Personal Blog API",
        Description = "A RESTful API for managing personal blog portfolio. This API provides endpoints for projects, skills, blog posts, education, experience, and contact messages.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "API Support",
            Email = "support@personalblog.com"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // XML Documentation dosyasını dahil et
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // JWT Bearer Authentication
    // Http scheme kullanarak kullanıcı sadece token'ı girer, "Bearer " prefix'i otomatik eklenir
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization. Enter only your token (without 'Bearer' prefix). The 'Bearer' prefix will be added automatically.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<PersonalBlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// JWT Settings - Environment variable öncelikli
// Öncelik sırası: Environment Variable > User Secrets > appsettings.Development.json > appsettings.json
var jwtSecretKey = builder.Configuration["JWT_SECRET_KEY"] 
    ?? builder.Configuration["JwtSettings:SecretKey"];

// JWT Settings'i yapılandır
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() 
    ?? new JwtSettings();

// Environment variable'dan SecretKey varsa, onu kullan
if (!string.IsNullOrWhiteSpace(jwtSecretKey))
{
    jwtSettings.SecretKey = jwtSecretKey;
}

// SecretKey validation - Güvenlik için zorunlu
if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
{
    throw new InvalidOperationException(
        "JWT SecretKey is required. " +
        "Please set JWT_SECRET_KEY environment variable, " +
        "configure User Secrets (dotnet user-secrets set \"JwtSettings:SecretKey\" \"your-key\"), " +
        "or add it to appsettings.json. " +
        "SecretKey must be at least 32 characters long for security.");
}

// Minimum uzunluk kontrolü (güvenlik)
if (jwtSettings.SecretKey.Length < 32)
{
    throw new InvalidOperationException(
        $"JWT SecretKey must be at least 32 characters long. Current length: {jwtSettings.SecretKey.Length}. " +
        "Please use a stronger secret key for production environments.");
}

// JWT Settings'i DI container'a kaydet
builder.Services.Configure<JwtSettings>(options =>
{
    options.SecretKey = jwtSettings.SecretKey;
    options.Issuer = jwtSettings.Issuer;
    options.Audience = jwtSettings.Audience;
    options.ExpirationInMinutes = jwtSettings.ExpirationInMinutes;
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };

    // JWT Authentication event'leri - Hata ayıklama için
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            
            logger.LogError(context.Exception, 
                "JWT Authentication failed. Exception: {ExceptionMessage}", 
                context.Exception.Message);
            
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            
            logger.LogWarning(
                "JWT Challenge occurred. Error: {Error}, ErrorDescription: {ErrorDescription}", 
                context.Error, context.ErrorDescription);
            
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation(
                "JWT Token validated successfully for user: {UserId}", 
                context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();

// Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// CORS Configuration logging
var corsLogger = app.Services.GetRequiredService<ILogger<Program>>();
var corsConfig = app.Services.GetRequiredService<IOptions<CorsSettings>>().Value;
if (corsConfig.AllowedOrigins.Length > 0)
{
    corsLogger.LogInformation(
        "CORS Policy configured with {OriginCount} allowed origin(s): {Origins}",
        corsConfig.AllowedOrigins.Length,
        string.Join(", ", corsConfig.AllowedOrigins));
}
else
{
    corsLogger.LogWarning("CORS Policy configured with AllowAnyOrigin (Development fallback only)");
}

// Global Exception Handler
app.UseGlobalExceptionHandler();

// CORS - Controller'lardan önce olmalı
app.UseCors("RestrictedCors");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Personal Blog API v1");
    options.RoutePrefix = "swagger";
    options.DisplayRequestDuration();
    options.EnableDeepLinking();
    options.EnableFilter();
    options.EnableTryItOutByDefault();
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    
    // Production'da gizlemek isterseniz:
    // if (app.Environment.IsDevelopment())
    // {
    //     // Swagger UI sadece development'ta
    // }
});

app.UseHttpsRedirection();

// Authentication ve Authorization - Sıra önemli!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
