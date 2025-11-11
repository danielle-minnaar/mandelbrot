using System.Numerics;

namespace Mandelbrot.Model.Parameters;

public record CalcParam
{
    public Complex InputPoint;
    public int Bound;
    public int MaxIterations;
    public bool isContinuous;
}