using Mandelbrot.src.Repositories;
using Mandelbrot.src.Repositories.Implementations;
using Mandelbrot.src.Singletons;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.AddEndpointsApiExplorer();

services.AddSwaggerGen(
    options =>
    {
        options.IncludeXmlComments(
            Path.Combine(AppContext.BaseDirectory, "Mandelbrot.xml"));
    });

services.AddControllers();

services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddScoped<IPaletteRepository, PaletteRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");