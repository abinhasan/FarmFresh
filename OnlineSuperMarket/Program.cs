using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuperMarket.Domain.DTO;
using SuperMarket.Domain.Entities.Contexts;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Domain.Interfaces;
using SuperMarket.Infrastructure.Data;
using SuperMarket.Infrastructure.Data.Seeds;
using SuperMarket.Services;
using SuperMarket.Services.Interfaces;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

#region Services & Repository inject
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddTransient<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IDataSeederRepository, DataSeederRepository>();
#endregion

RegisterCorsPolicies(builder.Services);

void RegisterCorsPolicies(IServiceCollection services)
{
    string[] localHostOrigins = new string[] {
               "http://localhost:4200", "https://localhost:4200"
            };

    string[] productionHostOrigins = new string[] {
                "http://localhost:4200", "https://localhost:4200"
            };

    services.AddCors(options =>    // CORS middleware must precede any defined endpoints
    {
        options.AddPolicy("DevelopmentCorsPolicy", builder =>
        {
            builder.WithOrigins(localHostOrigins)
                    .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
        options.AddPolicy("ProductionCorsPolicy", builder =>
        {
            builder.WithOrigins(productionHostOrigins)
                    .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
    });
}

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("UseDataSeed"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.SeedAsync(userManager, roleManager);
    await DefaultSuperAdmin.SeedAsync(userManager, roleManager);
    await DefaultBasicUser.SeedAsync(userManager, roleManager);
    SeedDatabase();
}

async void SeedDatabase() //can be placed at the very bottom under app.Run()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDataSeederRepository>();
        await dbInitializer.SeedData();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevelopmentCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
