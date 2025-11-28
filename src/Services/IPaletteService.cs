using System.Drawing.Imaging;
using Mandelbrot.src.Helpers.ColorKernels;

namespace Mandelbrot.src.Services;

/// <summary>
///     Interface for services that need to hold on to and modify
///     a <see cref="ColorPalette"/>.
/// </summary>
public interface IPaletteService
{
    /// <summary>
    ///     The currently active <see cref="ColorPalette"/>.
    /// </summary>
    ColorPalette CurrentPalette { get; }
    
    /// <summary>
    ///     The currently active <see cref="Helpers.ColorKernels.ColorType"/>.
    /// </summary>
    ColorType ColorType { get; }

    /// <summary>
    ///     Select a new <see cref="ColorPalette"/> to be the active color palette.
    /// </summary>
    /// <param name="paletteName">
    ///     The name of the new <see cref="ColorPalette"/>.
    /// </param>
    Task ChangePalette(string paletteName);

    /// <summary>
    ///     Select a new <see cref="Helpers.ColorKernels.ColorType"/> to be
    ///     the active type of coloring.
    /// </summary>
    /// <param name="newType">
    ///     The new type of coloring.
    /// </param>
    void ChangeColorType(ColorType newType);
    
    /// <summary>
    ///     Change one or both of dither ratio and color skew
    ///     on the active <see cref="Helpers.ColorKernels.ColorType"/>.
    /// </summary>
    /// <remarks>
    ///     A value of <c>null</c> leaves the variable unchanged.
    /// </remarks>
    /// <param name="newDither">
    ///     The new value of the dither ratio. Should be in range (0, 1].
    /// </param>
    /// <param name="newSkew">
    ///     The new value of the color skew. Should be in range [0, 1].
    /// </param>
    void NewVariables(double? newDither, double? newSkew);
    
    /// <summary>
    ///     Save any changes made to the <see cref="Helpers.ColorKernels.ColorType"/>
    ///     that is active to the db.
    /// </summary>
    Task SavePalette();

}