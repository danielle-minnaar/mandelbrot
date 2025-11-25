using System.Drawing;
using Mandelbrot.src.Model;
using Mandelbrot.src.Singletons;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mandelbrot.src.Repositories.Implementations;

/// <summary>
///     The repository of <see cref="ColorPalette"/> objects.
/// </summary>
public class PaletteRepository : IPaletteRepository
{
    private readonly AppSettings _settings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PaletteRepository"/> class.
    /// </summary>
    /// <param name="options">
    ///     The <see cref="AppSettings"/> that should be handled by dependecy injection.
    /// </param>
    public PaletteRepository(IOptions<AppSettings> options)
    {
        _settings = options.Value;
    }

    /// <inheritdoc/>
    public async Task<ColorPalette[]> GetAll()
    {
        var palettes = await Read();
        return palettes
            .OrderBy(palette => palette.LastUseTime)
            .ToArray();
    }

    /// <inheritdoc/>
    public async Task<ColorPalette> GetById(Guid id)
    {
        var palettes = await Read();
        var palette = palettes
            .Where(palette => palette.Id == id)
            .First()
            ?? throw new NullReferenceException($"Color palette with id: {id} not found.");
        
        palette.LastUseTime = DateTime.Now;
        await Update(palette);
        
        return palette;
    }

    /// <inheritdoc/>
    public async Task<ColorPalette> GetByName(string name)
    {
        var palettes = await Read();
        var palette = palettes
            .Where(palette => palette.Name == name)
            .First()
            ?? throw new NullReferenceException($"Color palette with name: {name} not found.");

        return palette;
    }

    /// <inheritdoc/>
    public async Task Update(ColorPalette item)
    {
        var palettes = await Read();
        var index = palettes.FindIndex(palette => palette.Name == item.Name);
        if (index == -1)
        {
            var m = $"Could not update the item with name: {item.Name}, because it was not found";
            throw new NullReferenceException(m);
        }

        palettes[index] = item;
        await Write(palettes);
    }

    /// <inheritdoc/>
    public async Task CreateItem(ColorPalette item)
    {
        item.Id = Guid.NewGuid();
        item.CreationTime = DateTime.Now;
        item.LastUseTime = DateTime.Now;

        var colorsPath = $"{_settings.ColorsLocation}/{item.Name}.png";
        if (!File.Exists(colorsPath))
        {
            var m = $"When trying to create color palette with name: {item.Name}, could not find colors file: {colorsPath}";
            throw new FileNotFoundException(m);
        }

        var palettes = await Read();
        palettes.Add(item);
        await Write(palettes);
    }

    private async Task<List<ColorPalette>> Read()
    {
        var jsonPalettes = string.Empty;
        var docPath = _settings.ColorPaletteDbLocation;
        if (!File.Exists(docPath))
        {
            var m = $"While reading could not find the db at: {docPath}";
            throw new FileNotFoundException(m);
        }

        using (var inputFile = new StreamReader(docPath))
        {
            var line = string.Empty;
            while ((line = await inputFile.ReadLineAsync()) != null)
            {
                jsonPalettes += line;
            }
        }

        var palettes = JsonConvert.DeserializeObject<List<ColorPalette>>(jsonPalettes)
            ?? [];
        
        foreach (var palette in palettes)
        {
            var colorPath = $"{_settings.ColorsLocation}/{palette.Name}.png";
            palette.Colors = GetColors(colorPath);
        }

        return palettes;
    }

    private async Task Write(List<ColorPalette> palettes)
    {
        string palettesJson = JsonConvert.SerializeObject(palettes);
        string docPath = _settings.ColorPaletteDbLocation;
        if (!File.Exists(docPath))
        {
            var m = $"While writing could not find the db at: {docPath}";
            throw new FileNotFoundException(m);
        }

        using (var outputFile = new StreamWriter(docPath))
        {
            await outputFile.WriteAsync(palettesJson);
        }
    }

    private Color[] GetColors(string path)
    {
        var colors = new Bitmap(path) ?? throw new NullReferenceException($"Could not find a color file at: {path}");
        var result = new Color[colors.Width];

        for (int i = 0; i < colors.Width; i++)
        {
            var color = colors.GetPixel(i, 0);
            result[i] = color;
        }

        return result;
    }
}