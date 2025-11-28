using System.Drawing;
using Mandelbrot.src.Dtos;
using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;

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
    ///     A new <see cref="ColorPalette"/> class, with empty Colors field.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public static ColorPalette ToModel(this ColorPalettePostDto palette)
    {
        if (palette.DitherRatio < 0 || palette.DitherRatio > 1)
        {
            var m = $"{nameof(palette.DitherRatio)} needs to be in range: [0, 1]";
            m += ", but the value is: {palette.DitherRatio}";
            throw new ArgumentException(m);
        }
        else if (palette.ColorSkew <= 0 || palette.ColorSkew > 1)
        {
            var m = $"{nameof(palette.ColorSkew)} needs to be in range: (0, 1]";
            m += $", but the value is: {palette.ColorSkew}";
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

    /// <summary>
    ///     Extension method to convert this <see cref="SpaceDto"/>
    ///     into a <see cref="SpaceParam"/>.
    /// </summary>
    /// <param name="dto">
    ///     The <see cref="SpaceDto"/> to be converted.
    /// </param>
    /// <returns>
    ///     A new <see cref="SpaceParam"/> class.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public static SpaceParam ToModel(this SpaceDto dto)
    {
        if (dto.XScale <= 0)
        {
            var m = $"Invalid value for {nameof(dto.XScale)}. ";
            m += $"It needs to be in range (0, inf), but the value was: {dto.XScale}";
            throw new ArgumentException(m);
        }
        else if (dto.XResolution <= 0 || dto.YResolution <= 0)
        {
            var m = $"Invalid resolution: ({dto.XResolution}, {dto.YResolution}). ";
            m += "Values need to be in range (0, inf)";
            throw new ArgumentException(m);
        }

        var yScale = dto.XScale * dto.YResolution / dto.XResolution;        
        return new SpaceParam
        {
            XMin = dto.Center.Real - dto.XScale / 2,
            XMax = dto.Center.Real + dto.XScale / 2,
            YMin = dto.Center.Imaginary - yScale / 2,
            YMax = dto.Center.Imaginary + yScale / 2,
            XSize = dto.XResolution,
            YSize = dto.YResolution
        };
    }

    /// <summary>
    ///     Extension method to turn this <see cref="Bitmap"/>
    ///     into an array of <c>byte</c>s that is safe to transfer
    ///     as a dto along an endpoint.
    /// </summary>
    /// <param name="model">
    ///     The <see cref="Bitmap"/> to be converted.
    /// </param>
    /// <returns>
    ///     A new array of <c>byte</c>s.
    /// </returns>
    public static byte[] ToByteArray(this Bitmap model)
    {
        using (var ms = new MemoryStream())
        {
            model.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
    
}