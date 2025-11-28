using Mandelbrot.src.Repositories;
using Mandelbrot.src.Repositories.Implementations;
using Mandelbrot.src.Services;
using Mandelbrot.src.Services.Implementations;
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

services.AddSwaggerGenNewtonsoftSupport();

services.AddControllers().AddNewtonsoftJson();

services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddSingleton<IPaletteRepository, PaletteRepository>();

services.AddSingleton<ICalculationService, CalculationService>();
services.AddSingleton<IImageService, ImageService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

app.Run();