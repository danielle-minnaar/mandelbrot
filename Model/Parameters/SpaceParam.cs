using System.Numerics;

namespace Mandelbrot.Model.Parameters;

/// <summary>
///     Class that contains meta data about a plane of complex numbers.
/// </summary>
public class SpaceParam
{
    /// <summary>
    ///     The minimum value for the real component in the plane.
    /// </summary>
    public double XMin;
    
    /// <summary>
    ///     The maximum value for the real component in the plane.
    /// </summary>
    public double XMax;
    
    /// <summary>
    ///     The minimum value for the imaginary component in the plane.
    /// </summary>
    public double YMin;
    
    /// <summary>
    ///     The maximum value for the imaginary component in the plane.
    /// </summary>
    public double YMax;
    
    /// <summary>
    ///  The number of points along the real component.
    /// </summary>
    public int XSize;
    
    /// <summary>
    ///     The number of points along the imaginary component.
    /// </summary>
    public int YSize;

    public SpaceParam(Complex center, double xScale, int xResolution, int yResolution)
    {
        XMin = center.Real - xScale / 2;
        XMax = center.Real + xScale / 2;
        YMin = center.Imaginary - (xScale * yResolution / xResolution / 2);
        YMax = center.Imaginary + (xScale * yResolution / xResolution / 2);

        XSize = xResolution;
        YSize = yResolution;
    }
}