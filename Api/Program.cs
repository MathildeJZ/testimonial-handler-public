using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;

// Application Interfaces
using Testimonial.Application.Interfaces;
using Testimonial.Application.Features.Students;
using Testimonial.Application.Features.Testimonials;
using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features.Auth;
using Testimonial.Application.Features.Users;
using Testimonial.Application.Features.Teachers;


// Infrastructure Implementations
using Testimonial.Infrastructure.Features.Testimonials;
using Testimonial.Infrastructure.Features.Students;
using Testimonial.Infrastructure.Features.Auth;
using Testimonial.Infrastructure.Features.Users;
using Testimonial.Infrastructure.Features.Teachers;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Infrastructure.Identity;
using Testimonial.Application.Features.SubjectEntries;
using Testimonial.Application.Features.Classes;
using Testimonial.Infrastructure.Features.Classes;
using Testimonial.Infrastructure.Features.SubjectEntries;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// QuestPDF License
QuestPDF.Settings.License = LicenseType.Community;

//ConnectionString
var cs = builder.Configuration.GetConnectionString("DefaultConnection");


// ============================================================================
// Repositories
// ============================================================================
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITestimonialRepository, TestimonialRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

// ============================================================================
// Services
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<ITestimonialPdfGenerator, TestimonialPdfGenerator>();
builder.Services.AddScoped<ICreateTestimonialPdfUseCase, CreateTestimonialPdfUseCase>();
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<ISubjectEntryService, SubjectEntriesService>();
builder.Services.AddScoped<ISubjectEntryRepository, SubjectEntryRepository>();
// ============================================================================

// ============================================================================
// Database Context
// ============================================================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    var csFromIndex = builder.Configuration["ConnectionStrings:DefaultConnection"];

    if (string.IsNullOrWhiteSpace(cs))
        throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

// ============================================================================
// Identity
// ============================================================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

// ============================================================================
// Controllers
// ============================================================================
builder.Services.AddControllers();

// ============================================================================
// Swagger
// ============================================================================
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// ============================================================================
// CORS
// ============================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

// ============================================================================
// JWT Authentication
// ============================================================================
var jwtSection = builder.Configuration.GetSection("Jwt");
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];
var key = jwtSection["Key"];

if (string.IsNullOrWhiteSpace(key))
    throw new InvalidOperationException("Missing jwt:Key in configuration.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30),
        RoleClaimType = "roles" // hvis du vælger den løsning
    };
});

//  HER:
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("roles", "Admin"));
});


// ============================================================================
// Authorization + HttpContext
// ============================================================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ============================================================================
// Build App
// ============================================================================
var app = builder.Build();


app.UseRouting();

// ============================================================================
// Middleware
// ============================================================================
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");


// ============================================================================
// Seeding
// ============================================================================
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedAsync(userManager, roleManager);
}



if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
        .CreateLogger("TestDataSeed");

    var conn = db.Database.GetDbConnection();
    logger.LogInformation("Seeding check on DB: {Database} | DataSource: {DataSource}", conn.Database, conn.DataSource);

    try
    {
        await Testimonial.Infrastructure.Persistence.Seed.TestDataSeed.SeedAsync(db, logger);
        logger.LogInformation("Seeding finished.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Seeding FAILED.");
        throw;
    }
}


// ============================================================================
// Fonts
// ============================================================================
var fontPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "Assets",
    "Fonts",
    "SchoolFrontPage.ttf");

FontManager.RegisterFontWithCustomName(
    "SchoolFrontPage",
    File.OpenRead(fontPath));



app.Run();
