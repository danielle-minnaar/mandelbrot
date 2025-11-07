namespace Mandelbrot.Model;

/// <summary>
///     A dto that contains meta data and raw data about iterations.
/// </summary>
public record IterationData : LoopParam
{
    /// <summary>
    ///     Contains the raw data for iterations and escape speed of all points.
    /// </summary>
    public required CalcResult[,] CalculationResults;

    /// <summary>
    ///     Of all the points not in the fractal this is the maximum number of iterations.
    /// </summary>
    public required int MaxIterations;

    /// <summary>
    ///     Of all the points not in the fractal this is the minimum number of iterations.
    /// </summary>
    public required int MinIterations;

    /// <summary>
    ///     The number of points that lie in the fractal.
    /// </summary>
    public required int PointsInFractal;

    /// <summary>
    ///     The time it took to calculate the iterations
    ///     or the escape speeds if applicable.
    /// </summary>
    public TimeSpan CalculationTime;
}