using System.Numerics;

namespace Mandelbrot.Model.Parameters;

public record SequenceParam
{
    public Complex Center;
    public double StartingSize;
    public double FinalSize;
    public double ZoomFactor;
    public int XResolution;
    public int YResolution;
}