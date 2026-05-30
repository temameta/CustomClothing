using CustomClothing.api;
using CustomClothing.database;
using CustomClothing.interfaces;
using CustomClothing.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClothingService, ClothingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.MapGet("/", () => Results.Content(
        "<h1>Custom Clothing API</h1><p>Документация: <a href='/swagger'>Swagger</a></p>", 
        "text/html; charset=utf-8"
    ))
    .ExcludeFromDescription();

app.MapClothingEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();