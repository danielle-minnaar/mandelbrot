using System.Drawing;

namespace Mandelbrot.Model;

/// <summary>
///     Holds a Mandelbrot image alonside metadata.
/// </summary>
public class BrotImage
{
    /// <summary>
    ///     Color image of the fractal.
    /// </summary>
    public required Bitmap Image;
    
    /// <summary>
    ///     Contains the bound of the complex space of the image.
    /// </summary>
    public required ComplexSpaceParameters complexSpaceParameters;
    
    /// <summary>
    ///     Of all the points not in the fractal this is the minimum number of iterations.
    /// </summary>
    public required int MinIterations;
    
    /// <summary>
    ///     Of all the points not in the fractal this is the maximum number of iterations.
    /// </summary>
    public required int MaxIterations;
    
    /// <summary>
    ///     The maximum number of iterations to which a point will be calculated.
    /// </summary>
    public int? MaxCalculatedIterations;

    /// <summary>
    ///     Contains the raw data for the iterations.
    /// </summary>
    public int[,]? Iterations;

    /// <summary>
    ///     Contains the raw data for the escape speeds.
    /// </summary>
    public double[,]? EscapeSpeeds;
}