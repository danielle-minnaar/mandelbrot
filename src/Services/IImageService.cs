using Mandelbrot.src.Helpers.ColorKernels;
using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;

namespace Mandelbrot.src.Services;

/// <summary>
///     Interface for image services. Used to generate different types of
///     Mandelbrot images.
/// </summary>
public interface IImageService
{
    /// <summary>
    ///     Generate a Mandelbrot image of the specified size.
    /// </summary>
    /// <param name="inputParam">
    ///     The <see cref="SpaceParam"/> that details the complex space
    ///     that the image occupies.
    /// </param>
    /// <returns>
    ///     A <see cref="BrotImage"/> Mandelbrot image.
    /// </returns>
    Task<BrotImage> GetImage(SpaceParam inputParam);
}