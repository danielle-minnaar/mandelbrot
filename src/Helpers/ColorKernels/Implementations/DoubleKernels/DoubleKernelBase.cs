using System.Drawing;
using Mandelbrot.src.Model;

namespace Mandelbrot.src.Helpers.ColorKernels.Implementations.DoubleKernels;

/// <summary>
///     The base class that all color kernels that use doubles inherit from.
/// </summary>
public abstract class DoubleKernelBase : KernelBase
{
    /// <summary>
    ///     The escape speed data that the kernel is applied to.
    /// </summary>
    protected readonly double[,] _escapeSpeeds;
    
    /// <summary>
    ///     The threshold values that are used to determine the spacing of colors.
    /// </summary>
    protected readonly double[] _escapeSpeedThresholds;

    /// <summary>
    ///     Base constructor that all color kernels based on doubles use.
    /// </summary>
    /// <param name="iterData">
    ///     The raw data.
    /// </param>
    /// <param name="palette">
    ///     The color palette.
    /// </param>
    /// <param name="type">
    ///     The type of coloring.
    /// </param>
    /// <exception cref="ArgumentNullException"></exception>
    protected DoubleKernelBase(
        IterationData iterData,
        ColorPalette palette,
        ColorType type) : base(iterData, palette, type)
    {
        _escapeSpeeds = iterData.EscapeSpeeds ??
            throw new ArgumentNullException(nameof(iterData));
        _escapeSpeedThresholds = GetUnevenEscapeSpeedThresholds(palette.ColorSkew);
        
    }

    /// <summary>
    ///     Get the colorId based on escape speed.
    /// </summary>
    /// <param name="speed">
    ///     The supplied escape speed.
    /// </param>
    /// <returns>
    ///     The index for the colorPalette, includes a fractional component
    ///     that indicates how close it is to this index and the next one.
    /// </returns>
    protected (int, double) GetFractionalColorId(double speed)
    {
        var colorId = 0;
        var lowerValue = _escapeSpeedThresholds[0];
        var higherValue = _escapeSpeedThresholds[^1];

        if (speed > higherValue)
        {
            return (_escapeSpeedThresholds.Length - 1, 0);
        }

        for (int i = 0; i < _escapeSpeedThresholds.Length; i++)
        {
            var threshold = _escapeSpeedThresholds[i];

            // Gets the largest value smaller than speed.
            if (threshold < speed && threshold > lowerValue)
            {
                lowerValue = threshold;
                colorId = Math.Min(i, _palette.Colors.Length - 1);
            }

            // Gets the smallest value greater than speed.
            else if (threshold >= speed && threshold < higherValue)
            {
                higherValue = threshold;
            }
        }

        var fraction = (speed - lowerValue) / (higherValue - lowerValue);

        return (colorId, fraction);
    }

    /// <summary>
    ///     Get the escape speed thresholds based on the escape speeds.
    /// </summary>
    /// <param name="skew"></param>
    /// <returns></returns>
    private double[] GetUnevenEscapeSpeedThresholds(double skew)
    {
        var numberOfBins = _palette.Colors.Length - 1;
        var binSizes = new int[numberOfBins];
        var queryable = _escapeSpeeds.Cast<double>();
        var count = queryable.Where(speed => speed != 0).Count();
        var standardBinSize = count / numberOfBins;

        for (int i = 0; i < numberOfBins; i++)
        {
            // as i progresses in [0, numberOfBins), offsetFactor will progress in [-1, 1].
            var offsetFactor = (double)i / (numberOfBins - 1) * 2 - 1;
            var offset = standardBinSize * (1 - skew) * offsetFactor;
            binSizes[i] = standardBinSize - (int)offset;
        }

        // ensures that binSizes accounts for the exact number of pixels that need a color.
        binSizes[0] += count - binSizes.Sum() - 1;

        var thresholdId = 0;
        var thresholdIds = new List<int> { thresholdId };

        foreach (var binSize in binSizes)
        {
            thresholdId += binSize;
            thresholdIds.Add(thresholdId);
        }

        var result = queryable
            .Where(speed => speed != 0)
            .Order()
            .Where((speed, index) => thresholdIds.Contains(index))
            .ToArray();

        return result;
    }
}