using System.Numerics;

namespace Mandelbrot.Model.Parameters;

/// <summary>
///     A dto that contains the necessary information to
///     perform a mandelbrot calculation on a single point.
/// </summary>
public record CalcParam
{
    /// <summary>
    ///     The point on which the calculation is performed.
    /// </summary>
    public Complex InputPoint;
    
    /// <summary>
    ///     The boundary value for the calculation.
    /// </summary>
    public int Bound;
    
    /// <summary>
    ///     The maximum iterations after which the point lies inside the fractal
    /// </summary>
    public int MaxIterations;
    
    /// <summary>
    ///     If true also calulate escape speed in addition to iterations.
    /// </summary>
    public bool isContinuous;
}