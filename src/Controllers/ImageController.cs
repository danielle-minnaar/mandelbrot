using Mandelbrot.src.Dtos;
using Mandelbrot.src.Exceptions;
using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mandelbrot.src.Controllers;

/// <summary>
///     Controller for managing Mandelbrot images.
/// </summary>
[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private readonly IImageService _images;

    /// <summary>
    ///     Initializes a new instance of the 
    ///     <see cref="ImageController"/> class.
    /// </summary>
    /// <param name="images">
    ///     The <see cref="IImageService"/>, handled by DI.
    /// </param>
    public ImageController(IImageService images)
    {
        _images = images;
    }

    /// <summary>
    ///     Starts the generation process on an image matching
    ///     the provided spatial parameter.
    /// </summary>
    /// <param name="spaceOfImage">
    ///     The spatial parameter of the image.
    /// </param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<string>> GenerateImage([FromBody] SpaceDto spaceOfImage)
    {
        try
        {
            await _images.GenerateImage(spaceOfImage.ToModel());
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    /// <summary>
    ///     Retrieve the progress of the current image generation.
    /// </summary>
    /// <returns>
    ///     The progress in whole percentage points.
    ///     A value of 0 indicates no generation is in progress.
    /// </returns>
    [HttpGet("progress")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public ActionResult<string> GetProgress()
    {
        var result =(int) (_images.GetProgress() * 100);
        return Ok(result);
    }

    /// <summary>
    ///     Retrieve the most recently generated image.
    /// </summary>
    [HttpGet]
    [Produces("image/png")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public ActionResult GetImage()
    {
        try
        {
            var result = _images.GetImage();
            return File(result.Image.ToByteArray(), "image/png");
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }
}