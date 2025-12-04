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
    [ProducesResponseType<string>(StatusCodes.Status202Accepted)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    public ActionResult<string> GenerateImage([FromBody] SpaceDto spaceOfImage)
    {
        Console.WriteLine(spaceOfImage);
        try
        {
            var resultId = _images.GenerateImage(spaceOfImage.ToModel());
            var locationUrl = Url.Action(nameof(GetImage), new {imGuid = resultId});
            return Accepted(locationUrl);
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

    /// <summary>
    ///     Retrieve an image with a specific id.
    /// </summary>
    /// <param name="imGuid">
    ///     The id of the image.
    /// </param>
    [HttpGet("{imGuid}")]
    [Produces("image/png")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public ActionResult GetImage(Guid imGuid)
    {
        try
        {
            var result = _images.GetImage(imGuid);
            return File(result.Image.ToByteArray(), "image/png");
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    ///     Recolor and retrieve an image based on the
    ///     most recently generated iteration data.
    /// </summary>
    [HttpPut()]
    [Produces("image/png")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public ActionResult GetRecoloredImage()
    {
        try
        {
            var result = _images.GetRecoloredImage();
            return File(result.Image.ToByteArray(), "image/png");
        }
        catch (NullReferenceException e)
        {
            return NotFound(e.Message);
        }
    }
}