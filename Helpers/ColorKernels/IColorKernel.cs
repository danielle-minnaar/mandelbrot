using System.Drawing;

namespace Mandelbrot.Helpers.ColorKernels;

/// <summary>
///     Interface for coloring kernels.
/// </summary>
/// <remarks>
///     The kernels hold the raw data that they are applied to themselves.
/// </remarks>
public interface IColorKernel
{
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
    Color Apply(int x, int y);

    /// <summary>
    ///     Get the size of the image that this kernel is applied to.
    /// </summary>
    /// <returns>
    ///     The size (xSize, ySize).
    /// </returns>
    (int, int) GetImageSize();
}