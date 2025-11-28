namespace Mandelbrot.src.Model.Parameters;

/// <summary>
///     Class that contains meta data about points on a plane of complex numbers.
/// </summary>
public record SpaceParam
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
}