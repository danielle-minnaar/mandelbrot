using System.Drawing;

namespace Mandelbrot.src.Dtos;

/// <summary>
///     A Dto to represent a color palette to the user.
/// </summary>
public record ColorPaletteDto
{
    /// <summary>
    ///     The id of the palette.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    ///     The name of the palette.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    ///     The colors in the palette.
    /// </summary>
    public required Color[] Colors { get; set; }
    

    /// <summary>
    ///     Determines the relative sizes of different colors.
    ///     <para>
    ///     A value of 1 means all colors are represented equally.
    ///     A value of x means that the color corresponding to the data with the
    ///     highest escape speed is present x times as much as the average color.
    ///     All other colors are sacel linearly.
    ///     </para>
    ///     <para>
    ///     Value is in range (0, 1].
    ///     </para>
    /// </summary>
    public required double ColorSkew { get; set; }
    
    /// <summary>
    ///     Determines the relative size of the area that is affected by dithering.
    ///     <para>
    ///     Value is in range [0, 1].
    ///     </para>
    /// </summary>
    public required double DitherRatio { get; set; }
}