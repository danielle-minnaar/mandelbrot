using System.Drawing;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

public class BandedKernel : IColorKernel
{
    private readonly int[,] _iterations;
    private readonly int _minIterations;
    private readonly int _maxIterations;
    private readonly Color[] _colorPalette;
    public BandedKernel(int[,] iterations, Color[] colorPalette)
    {
        _iterations = iterations;
        _colorPalette = colorPalette;
        
        var queryable = _iterations.Cast<int>();
        var count = queryable
            .Where(iter => iter != 0)
            .Count();
        _minIterations = queryable
            .Where(iter => iter != 0)
            .Min();
        _maxIterations = queryable
            .Where(iter => iter != 0)
            .Order()
            .SkipLast((int)(count * 0.1))
            .Last();
    }

    public Color Apply(int x, int y)
    {
        var iteration = _iterations[x, y];

        if (iteration == 0)
        {
            return Color.Black;
        }

        var scaledIteration = iteration - _minIterations + 1;
        var slope = 2d / _colorPalette.Length;
        var divisor = 1 + Math.Pow(Math.E, -1 * slope * scaledIteration);
        var colorId = (2 / divisor - 1) * _colorPalette.Length;
        colorId = Math.Min(colorId, _colorPalette.Length - 1);

        return _colorPalette[(int)colorId];
    }
}