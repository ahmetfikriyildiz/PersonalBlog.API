using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.Middlewares;
using PersonalBlog.API.Repositories.Implementations;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Implementations;
using PersonalBlog.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

    // XML Documentation dosyasýný dahil et
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // JWT Bearer Authentication için (ileride kullanýlacak)
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});
builder.Services.AddDbContext<PersonalBlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

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

var app = builder.Build();

// Global Exception Handler
app.UseGlobalExceptionHandler();

// CORS - Controller'lardan önce olmalý
app.UseCors("AllowAll");

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

app.UseAuthorization();

app.MapControllers();

app.Run();
