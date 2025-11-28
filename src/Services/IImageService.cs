using Mandelbrot.src.Exceptions;
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
    /// <exception cref="InOperationException"></exception>
    Task GenerateImage(SpaceParam inputParam);

    /// <summary>
    ///     Get the most recently generated Mandelbrot image.
    /// </summary>
    /// <returns>
    ///     The <see cref="BrotImage"/> that was last generated.
    /// </returns>
    /// <exception cref="NullReferenceException"></exception>
    BrotImage GetImage();

    /// <summary>
    ///     Generate and get a new color image based on
    ///     existing <see cref="IterationData"/>.
    /// </summary>
    /// <returns>
    ///     The newly generated <see cref="BrotImage"/>.
    /// </returns>
    Task<BrotImage> GetRecoloredImage();

    /// <summary>
    ///     Get the progress on generation of the current image.
    /// </summary>
    /// <returns>
    ///     A <c>double</c> in range [0, 1].
    ///     A value of 0 means no generation is in progress.
    /// </returns>
    double GetProgress();
}