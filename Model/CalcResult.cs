namespace Mandelbrot.Model;

public record CalcResult
{
    public required int Iterations;
    public double? EscapeSpeed;
}