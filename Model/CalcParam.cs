using System.Numerics;

namespace Mandelbrot.Model;

public record CalcParam
{
    public Complex InputPoint;
    public int Bound;
    public int MaxIterations;
    public bool isContinuous;
}