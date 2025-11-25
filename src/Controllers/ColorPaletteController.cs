using Mandelbrot.src.Model;
using Mandelbrot.src.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mandelbrot.src.Controllers;

/// <summary>
///     Controller for managing color palettes
/// </summary>
[ApiController]
[Route("api/colorpalettes")]
public class ColorPaletteController : ControllerBase
{
    private readonly IPaletteRepository _colorPalettes;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ColorPaletteController"/> class.
    /// </summary>
    /// <param name="colorPalettes">
    ///     The color palettes repository, should be handled by DI.
    /// </param>
    public ColorPaletteController(IPaletteRepository colorPalettes)
    {
        _colorPalettes = colorPalettes;
    }

    /// <summary>
    ///     Get all color palettes.
    /// </summary>
    /// <returns>
    ///     A JSON formatted array of <see cref="ColorPalette"/>.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<string>> GetAll()
    {
        ColorPalette[] result;
        try
        {
            result = await _colorPalettes.GetAll();
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        return Ok(JsonConvert.SerializeObject(result));
        
    }

    [HttpPost]
    public async Task Post([FromBody] ColorPalette palette)
    {
        await _colorPalettes.CreateItem(palette);
    }
}