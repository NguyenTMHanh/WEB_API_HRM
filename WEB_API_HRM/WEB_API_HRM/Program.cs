using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WEB_API_HRM.Data;
using WEB_API_HRM.Repositories;
using Microsoft.AspNetCore.Authorization;
using WEB_API_HRM.Helpers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "HRM API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<HRMContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<ApplicationRole>>();

// Cấu hình DbContext
builder.Services.AddDbContext<HRMContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("HRM"));
});

// Cấu hình AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Cấu hình HttpClient cho proxy
builder.Services.AddHttpClient();

// Cấu hình Authentication JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// Đăng ký các repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRankRepository, RankRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IJobTitleRepository, JobTitleRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICheckInOutSettingRepository, CheckInOutRepository>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IJobTypeRepository, JobTypeRepository>();
builder.Services.AddScoped<IRateInsuranceRepository, RateInsuranceRepository>();
builder.Services.AddScoped<ITaxRateProgressionRepository, TaxRateProgressionRepository>();
builder.Services.AddScoped<IDeductionLevelRepository, DeductionLevelRepository>();
builder.Services.AddScoped<ISalaryCoefficientRepository, SalaryCoefficientRepository>();
builder.Services.AddScoped<IAllowanceRepository, AllowanceRepository>();
builder.Services.AddScoped<IMinimumWageAreaRepository, MinimumWageAreaRepository>();
builder.Services.AddScoped<IBasicSettingSalaryRepository, BasicSettingSalaryRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
// Cấu hình Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewRoles", policy =>
        policy.AddRequirements(new PermissionRequirement("permission", "view")));

    options.AddPolicy("CanCreateRoles", policy =>
        policy.AddRequirements(new PermissionRequirement("permission", "create")));

    options.AddPolicy("CanUpdateRoles", policy =>
        policy.AddRequirements(new PermissionRequirement("permission", "update")));

    options.AddPolicy("CanDeleteRoles", policy =>
        policy.AddRequirements(new PermissionRequirement("permission", "delete")));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewSettings", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "view")));

    options.AddPolicy("CanCreateSettings", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "create")));

    options.AddPolicy("CanUpdateSettings", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "update")));

    options.AddPolicy("CanDeleteSettings", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "delete")));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewEmployees", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "view")));

    options.AddPolicy("CanCreateEmployees", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "create")));

    options.AddPolicy("CanUpdateEmployees", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "update")));

    options.AddPolicy("CanDeleteEmployees", policy =>
        policy.AddRequirements(new PermissionRequirement("setting", "delete")));
});


// Đăng ký PermissionAuthorizationHandler
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Khởi tạo database và seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<HRMContext>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

    context.Database.EnsureCreated();
    await DbInitializer.SeedAsync(context, roleManager);
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Proxy endpoints
app.MapGet("/proxy/api/provinces", async ([FromServices] IHttpClientFactory httpClientFactory) =>
{
    try
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://provinces.open-api.vn/api/p?depth=1");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        return Results.Content(data, "application/json");
    }
    catch (HttpRequestException ex)
    {
        return Results.Problem($"Error fetching provinces: {ex.Message}", statusCode: 500);
    }
}).RequireAuthorization();

app.MapGet("/proxy/api/provinces/{code}", async ([FromServices] IHttpClientFactory httpClientFactory, int code) =>
{
    try
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://provinces.open-api.vn/api/p/{code}?depth=2");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        return Results.Content(data, "application/json");
    }
    catch (HttpRequestException ex)
    {
        return Results.Problem($"Error fetching districts: {ex.Message}", statusCode: 500);
    }
}).RequireAuthorization();

app.MapGet("/proxy/api/districts/{code}", async ([FromServices] IHttpClientFactory httpClientFactory, int code) =>
{
    try
    {
        var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://provinces.open-api.vn/api/d/{code}?depth=2");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        return Results.Content(data, "application/json");
    }
    catch (HttpRequestException ex)
    {
        return Results.Problem($"Error fetching wards: {ex.Message}", statusCode: 500);
    }
}).RequireAuthorization();

app.MapControllers();

app.Run();