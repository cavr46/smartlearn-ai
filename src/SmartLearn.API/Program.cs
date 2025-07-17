using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartLearn.Application.Commands;
using SmartLearn.Domain.Entities;
using SmartLearn.Domain.Interfaces;
using SmartLearn.Infrastructure.Authentication;
using SmartLearn.Infrastructure.Data;
using SmartLearn.Infrastructure.Repositories;
using SmartLearn.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// JWT Token Service
builder.Services.AddScoped<JwtTokenService>(provider =>
    new JwtTokenService(
        secretKey,
        jwtSettings["Issuer"] ?? "",
        jwtSettings["Audience"] ?? "",
        int.Parse(jwtSettings["ExpiryMinutes"] ?? "60")
    ));

// AI Services
builder.Services.AddScoped<IQuizGenerationService, QuizGenerationService>();
builder.Services.AddScoped<ITranscriptionService, TranscriptionService>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    
    await context.Database.EnsureCreatedAsync();
    await SeedDataAsync(userManager, roleManager);
}

app.Run();

static async Task SeedDataAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
{
    // Create roles
    string[] roles = { "Admin", "Instructor", "Student" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
        }
    }

    // Create admin user
    if (await userManager.FindByEmailAsync("admin@smartlearn.ai") == null)
    {
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Admin",
            LastName = "User",
            UserName = "admin@smartlearn.ai",
            Email = "admin@smartlearn.ai",
            Role = UserRole.Admin,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // Create instructor user
    if (await userManager.FindByEmailAsync("instructor@smartlearn.ai") == null)
    {
        var instructorUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Instructor",
            UserName = "instructor@smartlearn.ai",
            Email = "instructor@smartlearn.ai",
            Role = UserRole.Instructor,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(instructorUser, "Instructor123!");
        await userManager.AddToRoleAsync(instructorUser, "Instructor");
    }

    // Create student user
    if (await userManager.FindByEmailAsync("student@smartlearn.ai") == null)
    {
        var studentUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Student",
            UserName = "student@smartlearn.ai",
            Email = "student@smartlearn.ai",
            Role = UserRole.Student,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(studentUser, "Student123!");
        await userManager.AddToRoleAsync(studentUser, "Student");
    }
}