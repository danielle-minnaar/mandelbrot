using System.Drawing;
using Mandelbrot.Model;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

/// <summary>
///     A coloring kernel that produces color bands based on number of iterations.
/// </summary>
public class BandedKernel : KernelBase
{
    private readonly int[,] _iterations;
    private readonly int _minIterations;
    private readonly int _maxIterations;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BandedKernel"/> class.
    /// </summary>
    /// <param name="iterData">
    ///     The raw data held in a <see cref="IterationData"/>.
    /// </param>
    /// <param name="colorPalette">
    ///     The color palette
    /// </param>
    public BandedKernel(
        IterationData iterData,
        Color[] colorPalette) : base(iterData, colorPalette)
    {
        _iterations = iterData.Iterations;
        
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

    /// <inheritdoc/>
    public override Color Apply(int x, int y)
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