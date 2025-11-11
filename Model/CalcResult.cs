namespace Mandelbrot.Model;

/// <summary>
///     A dto that holds the result of the calculations for a single point.
/// </summary>
public record CalcResult
{
    /// <summary>
    ///     The number of iterations required to escape the bound value.
    /// </summary>
    public required int Iterations;
    
    /// <summary>
    ///     How quickly a point escapes the bound value.
    /// </summary>
    public double? EscapeSpeed;
}