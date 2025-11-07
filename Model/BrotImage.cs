using System.Drawing;

namespace Mandelbrot.Model;

/// <summary>
///     Holds a Mandelbrot image alonside metadata.
/// </summary>
public record BrotImage
{
    /// <summary>
    ///     Color image of the fractal.
    /// </summary>
    public required Bitmap Image;

    /// <summary>
    ///     The time it took to convert from raw data to a color image.
    /// </summary>
    public required TimeSpan ColorTime;

    /// <summary>
    ///     Holds raw iteration data as well as iteration meta data.
    /// </summary>
    public required IterationData IterationData;
}