using Newtonsoft.Json;

namespace Mandelbrot.src.Dtos;

/// <summary>
///     A dto containing optional fields for modifying the active color palette.
/// </summary>
public record ColorPalettePatchDto
{
    /// <summary>
    ///     The new color skew of the active color palette, which
    ///     determines the relative sizes of different colors.
    ///     <para>
    ///     A value of 1 means all colors are represented equally.
    ///     A value of x means that the color corresponding to the data with the
    ///     highest escape speed is present x times as much as the average color.
    ///     All other colors are sacel linearly.
    ///     </para>
    ///     <para>
    ///     Value needs to be in range (0, 1]. Default value is null.
    ///     </para>
    /// </summary>
    [JsonProperty("color_skew")]
    public double? ColorSkew { get; set; }
    
    /// <summary>
    ///     The new dither ratio of the active color palette, which
    ///     determines the relative size of the area that is affected by dithering.
    ///     <para>
    ///     Value needs to be range [0, 1]. Default value is null.
    ///     </para>
    /// </summary>
    [JsonProperty("dither_ratio")]
    public double? DitherRatio { get; set; }
}