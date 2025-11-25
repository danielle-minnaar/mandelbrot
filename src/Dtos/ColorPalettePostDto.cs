using Newtonsoft.Json;

namespace Mandelbrot.src.Dtos;

/// <summary>
///     A dto containing the required fields to create a new color palette.
/// </summary>
public record ColorPalettePostDto
{
    /// <summary>
    ///     The name of the new color palette.
    /// </summary>
    [JsonProperty("name")]
    public required string Name { get; set; }
    
    /// <summary>
    ///     The color skew of the new color palette, which
    ///     determines the relative sizes of different colors.
    ///     <para>
    ///     A value of 1 means all colors are represented equally.
    ///     A value of x means that the color corresponding to the data with the
    ///     highest escape speed is present x times as much as the average color.
    ///     All other colors are sacel linearly.
    ///     </para>
    ///     <para>
    ///     Value needs to be in range (0, 1]. Default value is 1.
    ///     </para>
    /// </summary>
    public required double ColorSkew { get; set; } = 1;
    
    /// <summary>
    ///     The dither ratio of the new color palette, which
    ///     determines the relative size of the area that is affected by dithering.
    ///     <para>
    ///     Value needs to be range [0, 1]. Default value is 0.
    ///     </para>
    /// </summary>
    public required double DitherRatio { get; set; } = 0;
}