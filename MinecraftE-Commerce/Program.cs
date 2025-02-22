using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Infrastructure.Data;
using MinecraftE_Commerce.Infrastructure.Repositories;
using MinecraftE_Commerce.Infrastructure.Services.TokenServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAnnoucementService, AnnouncementRepo>();
builder.Services.AddScoped<ITokenService, GenerateTokenJwt>();

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
