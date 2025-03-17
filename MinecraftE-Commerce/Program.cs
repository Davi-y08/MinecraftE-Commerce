using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infra.Services;
using MinecraftE_Commerce.Infrastructure.Data;
using MinecraftE_Commerce.Infrastructure.Repositories;
using MinecraftE_Commerce.Infrastructure.Services.TokenServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IAnnoucementService, AnnouncementRepo>();
builder.Services.AddScoped<ITokenService, GenerateTokenJwt>();
builder.Services.AddScoped<IMailService, MailSender>();
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultSignOutScheme =
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var issuer = builder.Configuration["JWT:Issuer"];
    var audience = builder.Configuration["JWT:Audience"];
    var key = builder.Configuration["JWT:Key"];

    if (issuer == null || audience == null || key == null)
    {
        throw new ArgumentException("Failed in .json file");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(cors =>
{
    cors.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials().WithOrigins("http://localhost:5174").WithOrigins("http://localhost:5174");
});

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Pfps")),
    RequestPath = "/Pfps"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ImagesAnnouncements")),
    RequestPath = "/ImagesAnnouncements"
});


app.MapControllers();

app.Run();
