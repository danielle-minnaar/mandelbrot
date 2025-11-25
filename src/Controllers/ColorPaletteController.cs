using Mandelbrot.src.Dtos;
using Mandelbrot.src.ExtensionMethods;
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
    ///     A JSON formatted array of <see cref="ColorPaletteDto"/>.
    /// </returns>
    [HttpGet]
    [ProducesResponseType<ColorPaletteDto[]>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GetAll()
    {
        ColorPalette[] palettes;
        try
        {
            palettes = await _colorPalettes.GetAll();
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }

        var result = palettes.Select(palette => palette.ToDto()).ToList();
        
        return Ok(JsonConvert.SerializeObject(result));
        
    }

    /// <summary>
    ///     Create a new color palette. There needs to already be a .png file of the colors whith the same name.
    /// </summary>
    /// <param name="palette">
    ///     The <see cref="ColorPalettePostDto"/> used to create the new color palette.
    /// </param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromBody] ColorPalettePostDto palette)
    {
        try
        {
            await _colorPalettes.CreateItem(palette.ToModel());
            return Created();
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}