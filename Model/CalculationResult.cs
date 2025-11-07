namespace Mandelbrot.Model;

public record CalculationResult
{
    public required int[,] Iterations;
    public double[,]? EscapeSpeeds;
}