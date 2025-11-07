namespace Mandelbrot.Model;

/// <summary>
///     A dto that contains meta data and raw data about iterations.
/// </summary>
public record IterationData
{
    /// <summary>
    ///     Contains the boundaries of the complex space of the image.
    /// </summary>
    public required ComplexSpaceParameters ComplexSpaceParameters;

    /// <summary>
    ///     The maximum number of iterations to which a point has been calculated.
    /// </summary>
    public int MaxCalculatedIterations;

    /// <summary>
    ///     The boundary value beyond which a point is decided to lie outside the fractal.
    /// </summary>
    public int Bound;

    /// <summary>
    ///     Of all the points not in the fractal this is the maximum number of iterations.
    /// </summary>
    public int MaxIterations;

    /// <summary>
    ///     Of all the points not in the fractal this is the minimum number of iterations.
    /// </summary>
    public int MinIterations;

    /// <summary>
    ///     The number of points that lie in the fractal.
    /// </summary>
    public int PointsInFractal;

    /// <summary>
    ///     Contains the raw data for the iterations of all points.
    /// </summary>
    public int[,]? Iterations;

    /// <summary>
    ///     Contains the raw data for the escape speed of all points.
    /// </summary>
    public double[,]? EscapeSpeeds;

    /// <summary>
    ///     The time it took to calculate the iterations
    ///     or the escape speeds if applicable.
    /// </summary>
    public TimeSpan CalculationTime;
}