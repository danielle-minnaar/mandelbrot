using Mandelbrot.src.Dtos;
using Mandelbrot.src.Model;

namespace Mandelbrot.src.ExtensionMethods;

/// <summary>
///     Class holding the extension methods for converting between Dtos and Model objects.
/// </summary>
public static class DtoConversions
{
    /// <summary>
    ///     Extension method to convert this <see cref="ColorPalette"/> into a <see cref="ColorPaletteDto"/>.
    /// </summary>
    /// <param name="palette">
    ///     The <see cref="ColorPalette"/> to be converted.
    /// </param>
    /// <returns>
    ///     The <see cref="ColorPaletteDto"/> class.
    /// </returns>
    public static ColorPaletteDto ToDto(this ColorPalette palette)
    {
        return new ColorPaletteDto
        {
            Id = palette.Id,
            Name = palette.Name,
            Colors = palette.Colors,
            ColorSkew = palette.ColorSkew,
            DitherRatio = palette.DitherRatio
        };
    }

    /// <summary>
    ///     Extension method to convert this <see cref="ColorPalettePostDto"/>
    ///     into a <see cref="ColorPalette"/>.
    /// </summary>
    /// <remarks>
    ///     The Colors field of the <see cref="ColorPalette"/> will remain empty,
    ///     since this function is only meant to be used to create an object that
    ///     needs to be written to the db.
    /// </remarks>
    /// <param name="palette">
    ///     The <see cref="ColorPalettePostDto"/> to be converted.
    /// </param>
    /// <returns>
    ///     The <see cref="ColorPalette"/> class, with empty Colors field.
    /// </returns>
    public static ColorPalette ToModel(this ColorPalettePostDto palette)
    {
        if (palette.DitherRatio < 0 || palette.DitherRatio > 1)
        {
            var m = $"{nameof(palette.DitherRatio)} needs to be in range: [0, 1], but the value is: {palette.DitherRatio}";
            throw new ArgumentException(m);
        }
        else if (palette.ColorSkew <= 0 || palette.ColorSkew > 1)
        {
            var m = $"{nameof(palette.ColorSkew)} needs to be in range: (0, 1], but the value is: {palette.ColorSkew}";
            throw new ArgumentException(m);
        }
        return new ColorPalette
        {
            Id = Guid.NewGuid(),
            Name = palette.Name,
            ColorSkew = palette.ColorSkew,
            DitherRatio = palette.DitherRatio,
            CreationTime = DateTime.Now,
            LastUseTime = DateTime.Now
        };
    }
    
}