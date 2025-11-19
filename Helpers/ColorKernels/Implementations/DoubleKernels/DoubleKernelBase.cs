using System.Drawing;
using Mandelbrot.Model;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

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
    /// <param name="colorPalette">
    ///     The color palette.
    /// </param>
    /// <param name="colorSkew">
    ///     The skew in color.
    /// </param>
    /// <exception cref="ArgumentNullException"></exception>
    protected DoubleKernelBase(
        IterationData iterData,
        Color[] colorPalette,
        double colorSkew) : base(iterData, colorPalette)
    {
        _escapeSpeeds = iterData.EscapeSpeeds ??
            throw new ArgumentNullException(nameof(iterData));
        _escapeSpeedThresholds = GetUnevenEscapeSpeedThresholds(colorSkew);
        
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
    protected double GetFractionalColorId(double speed)
    {
        var colorId = 0;
        var lowerValue = _escapeSpeedThresholds[0];
        var higherValue = _escapeSpeedThresholds[^1];

        if (speed > higherValue)
        {
            return _escapeSpeedThresholds.Length - 1;
        }

        for (int i = 0; i < _escapeSpeedThresholds.Length; i++)
        {
            var threshold = _escapeSpeedThresholds[i];
            if (threshold < speed && threshold > lowerValue)
            {
                lowerValue = threshold;
                colorId = Math.Min(i, _colorPalette.Length - 1);
            }
            else if (threshold >= speed && threshold < higherValue)
            {
                higherValue = threshold;
            }
        }

        var fraction = (speed - lowerValue) / (higherValue - lowerValue);

        return colorId + fraction;
    }

    /// <summary>
    ///     Get the escape speed thresholds based on the escape speeds.
    /// </summary>
    /// <param name="skew"></param>
    /// <returns></returns>
    private double[] GetUnevenEscapeSpeedThresholds(double skew)
    {
        var numberOfBins = _colorPalette.Length - 1;
        var binSizes = new int[numberOfBins];
        var queryable = _escapeSpeeds.Cast<double>();
        var count = queryable.Where(speed => speed != 0).Count();
        var standardBinSize = count / numberOfBins;

        for (int i = 0; i < numberOfBins; i++)
        {
            // as i progresses from 0 to numberOfBins - 1, offsetFactor will go from -1 to 1.
            var offsetFactor = (double)i / (numberOfBins - 1) * 2 - 1;
            var offset = standardBinSize * (1 - skew) * offsetFactor;
            binSizes[i] = standardBinSize - (int)offset;
        }

        // ensures that the binSizes accounts for the exact number of pixels that need a color.
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