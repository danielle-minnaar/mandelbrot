namespace Mandelbrot.src.Helpers.ColorKernels;

/// <summary>
///     Contains the different types of coloring that a kernel can be.
/// </summary>
public enum ColorKernelType
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