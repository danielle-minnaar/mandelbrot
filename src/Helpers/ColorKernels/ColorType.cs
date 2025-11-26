using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Helpers.ColorKernels.Implementations;

namespace Mandelbrot.src.Helpers.ColorKernels;

/// <summary>
///     Contains the different types of coloring that a kernel can be.
/// </summary>
/// <remarks>
///     Has an extension method implemented in <see cref="DataConversions"/>
///     and is used in <see cref="KernelFactory"/>. These need to be updated
///     if another type is added!
/// </remarks>
public enum ColorType
{
    /// <summary>
    ///     This coloring has color bands with dithering at the edges.
    /// </summary>
    Dithered,
    
    /// <summary>
    ///     This coloring has no color bands and instead has a continuous
    ///     distribution of color.
    /// </summary>
    Continuous,
    
    /// <summary>
    ///     This coloring has discrete bands of colors.
    /// </summary>
    Banded
}