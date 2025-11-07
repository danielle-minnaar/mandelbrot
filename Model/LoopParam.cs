namespace Mandelbrot.Model;

/// <summary>
///     A dto that contains the meta data necessary to calculate the mandelbrot iterations.
/// </summary>
public record LoopParam
{
    /// <summary>
    ///     Contains the boundaries of the complex space of the image.
    /// </summary>
    public required ComplexSpaceParameters SpaceParam;

    /// <summary>
    ///     The boundary value beyond which a point is decided to lie outside the fractal.
    /// </summary>
    public required int Bound;

    /// <summary>
    ///     The maximum number of iterations to which a point has been calculated.
    /// </summary>
    public required int MaxCalculatedIterations;

    /// <summary>
    ///     If true iteration data is not limited to iterations
    ///     , but also includes the continuous escape speeds.
    /// </summary>
    public required bool IsContinuous;
}