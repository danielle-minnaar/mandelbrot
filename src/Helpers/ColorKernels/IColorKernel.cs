using Mandelbrot.src.Helpers.ColorKernels.Implementations;
using SkiaSharp;

namespace Mandelbrot.src.Helpers.ColorKernels;

/// <summary>
///     Interface for coloring kernels.
/// </summary>
/// <remarks>
///     Needs to be created by <see cref="KernelFactory"/> for safe instantiation!
/// </remarks>
public interface IColorKernel
{
    /// <summary>
    ///     Contains the type of coloring that this kernel uses.
    /// </summary>
    ColorType Type { get; init; }

    /// <summary>
    ///     Apply this kernel to the given x and y coordinates.
    /// </summary>
    /// <param name="x">
    ///     x coordinate corresponds to real values.
    /// </param>
    /// <param name="y">
    ///     y coordinate corresponds to imaginary values.
    /// </param>
    /// <returns>
    ///     The <c>Color</c> of this pixel.
    /// </returns>
    SKColor Apply(int x, int y);

    /// <summary>
    ///     Get the size of the image that this kernel is applied to.
    /// </summary>
    /// <returns>
    ///     The size (xSize, ySize).
    /// </returns>
    (int, int) GetImageSize();
}