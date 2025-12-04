using Mandelbrot.src.Repositories;
using Mandelbrot.src.Repositories.Implementations;
using Mandelbrot.src.Services;
using Mandelbrot.src.Services.Implementations;
using Mandelbrot.src.Singletons;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Location"));
});

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
services.AddSingleton<IPaletteService, PaletteService>();
services.AddSingleton<ICalculationService, CalculationService>();
services.AddSingleton<IImageService, ImageService>();

var app = builder.Build();

var provider = app.Services;
var service = provider.GetRequiredService<IPaletteService>();
await service.InitializeAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseCors("AllowAngularApp");

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

app.Run();