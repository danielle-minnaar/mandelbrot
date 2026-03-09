using System.Threading.Tasks;
using Mandelbrot.src.Dtos;
using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Helpers.ColorKernels;
using Mandelbrot.src.Repositories;
using Mandelbrot.src.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mandelbrot.src.Controllers;

/// <summary>
///     Controller for managing color palettes
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ColorPaletteController"/> class.
/// </remarks>
/// <param name="colorPalettes">
///     The color palettes repository, should be handled by DI.
/// </param>
/// <param name="activePalette">
///     The color palette service, should be handled by DI.
/// </param>
[ApiController]
[Route("api/colorpalettes")]
public class ColorPaletteController(
    IPaletteRepository colorPalettes,
    IPaletteService activePalette) : ControllerBase
{
    private readonly IPaletteRepository _colorPalettes = colorPalettes;
    private readonly IPaletteService _activePalette = activePalette;

    /// <summary>
    ///     Set the currently active color palette by name.
    /// </summary>
    /// <param name="paletteName">
    ///     The name of the new currently active color palette
    /// </param>
    [HttpPost("active/{paletteName}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ChangeActivePalette(string paletteName)
    {
        try
        {
            await _activePalette.ChangePalette(paletteName);
            return Created();
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    ///     Set the currently active color type.
    /// </summary>
    /// <param name="colorType">
    ///     The new color type.
    /// </param>
    [HttpPost("active/colortype/{colorType}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult ChangeActiveColorType(ColorType colorType)
    {
        _activePalette.ChangeColorType(colorType);
        return Created();
    }

    /// <summary>
    ///     Update values in the active color palette.
    /// </summary>
    /// <param name="newValues">
    ///     The new values.
    /// </param>
    [HttpPatch("active")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateActivePalette([FromBody] ColorPalettePatchDto newValues)
    {
        try
        {
            _activePalette.UpdatePalette(newValues);
            return Created();
        }
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Save changes to the active color palette to db.
    /// </summary>
    [HttpPost("active")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SaveActivePalette()
    {
        try
        {
            await _activePalette.SavePalette();
            return Created();
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
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
        try
        {
            var palettes = await _colorPalettes.GetAll();
            var result = palettes.Select(palette => palette.ToDto()).ToList();
            return Ok(JsonConvert.SerializeObject(result));
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    ///     Create a new color palette. There needs to already be a
    ///     .png file of the colors whith the same name.
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

    /// <summary>
    ///     Get a color palette by id.
    /// </summary>
    /// <param name="id">
    ///     The <see cref="Guid"/> of the color palette.
    /// </param>
    /// <returns>
    ///     A JSON formatted <see cref="ColorPaletteDto"/>.
    /// </returns>
    [HttpGet("id/{id}")]
    [ProducesResponseType<ColorPaletteDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GetById(Guid id)
    {
        try
        {
            var result = await _colorPalettes.GetById(id);
            return JsonConvert.SerializeObject(result.ToDto());
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    ///     Get a color palette by name.
    /// </summary>
    /// <param name="name">
    ///     The name of the color palette.
    /// </param>
    /// <returns>
    ///     A JSON formatted <see cref="ColorPaletteDto"/>.
    /// </returns>
    [HttpGet("name/{name}")]
    [ProducesResponseType<ColorPaletteDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GetByName(string name)
    {
        try
        {
            var result = await _colorPalettes.GetByName(name);
            return Ok(JsonConvert.SerializeObject(result.ToDto()));
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }
}