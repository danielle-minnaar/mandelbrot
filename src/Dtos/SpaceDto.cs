using System.Numerics;
using Newtonsoft.Json;

namespace Mandelbrot.src.Dtos;

/// <summary>
///     A dto that is used to specify the spatial parameters of
///     a Mandelbrot image.
/// </summary>
public record SpaceDto
{
    /// <summary>
    ///     A <see cref="Complex"/> point that represents
    ///     the center of the image.
    /// </summary>
    [JsonProperty("center")]
    public Complex Center { get; set; }
    
    /// <summary>
    ///     The size of the complex space of the image
    ///     along the real component.
    /// </summary>
    [JsonProperty("real_scale")]
    public double XScale { get; set; }
    
    /// <summary>
    ///     The width of the image in pixels.
    /// </summary>
    [JsonProperty("image_width")]
    public int XResolution { get; set; }
    
    /// <summary>
    ///     The height of the image in pixels.
    /// </summary>
    [JsonProperty("image_height")]
    public int YResolution { get; set; }
}