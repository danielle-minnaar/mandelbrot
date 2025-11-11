using System.Numerics;

namespace Mandelbrot.Model.Parameters;

/// <summary>
///     A dto that contains the meta data required to create
///     a sequence of <see cref="SpaceParam"/> that zooms in
///     on a single point.
/// </summary>
public record SequenceParam
{
    /// <summary>
    ///     The center that is zoomed in on.
    /// </summary>
    public Complex Center;

    /// <summary>
    ///     The x-scale of the first <see cref="SpaceParam"/> in the sequence.
    /// </summary>
    public double StartingSize;

    /// <summary>
    ///     The x-scale of the last <see cref="SpaceParam"/> in the sequence.
    /// </summary>
    public double FinalSize;

    /// <summary>
    ///     The degree to which each image in the sequence is zoomed
    ///     when compared to the last image.
    ///     Values should lie between 0 and 1 (exclusive).
    /// </summary>
    public double ZoomFactor;

    /// <summary>
    ///     The number of points along the real component.
    /// </summary>
    public int XResolution;
    
    /// <summary>
    ///     The number of points along the imaginary component.
    /// </summary>
    public int YResolution;
}