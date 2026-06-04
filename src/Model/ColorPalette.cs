using Newtonsoft.Json;
using SkiaSharp;

namespace Mandelbrot.src.Model;

/// <summary>
///     A model class that holds data and meta data used to turn iteration data into a color image.
/// </summary>
public class ColorPalette
{
    /// <summary>
    ///     The id of the palette.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id;
    
    /// <summary>
    ///     The name of the palette.
    /// </summary>
    [JsonProperty("name")]
    public required string Name { get; init; }
    
    /// <summary>
    ///     An array of <c>Color</c> used for coloring.
    /// </summary>
    [JsonIgnore]
    public SKColor[] Colors = [];
    
    /// <summary>
    ///     Determines the relative sizes of different colors.
    ///     <para>
    ///     A value of 1 means all colors are represented equally.
    ///     A value of x means that the color corresponding to the data with the
    ///     highest escape speed is present x times as much as the average color.
    ///     All other colors are sacel linearly.
    ///     </para>
    ///     Value is in range (0, 1].
    /// </summary>
    [JsonProperty("color_skew")]
    public required double ColorSkew;
    
    /// <summary>
    ///     Determines the relative size of the area that is affected by dithering.
    ///     <para>
    ///     Value is in range [0, 1].
    ///     </para>
    /// </summary>
    [JsonProperty("dither_ratio")]
    public required double DitherRatio;

    /// <summary>
    ///     The date time when this palette was created.
    /// </summary>
    [JsonProperty("creation_time")]
    public DateTime CreationTime;
    
    /// <summary>
    ///     The date time when this palette was last used.
    /// </summary>
    [JsonProperty("last_use_time")]
    public DateTime LastUseTime;
}