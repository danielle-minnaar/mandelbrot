using System.Drawing;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

public abstract class DoubleKernelBase : IColorKernel
{
    protected readonly double[,] _escapeSpeeds;
    protected readonly double[] _escapeSpeedThresholds;
    protected readonly Color[] _colorPalette;


    protected DoubleKernelBase(double[,] escapeSpeeds, Color[] colorPalette, double colorSkew)
    {
        _escapeSpeeds = escapeSpeeds;
        _colorPalette = colorPalette;
        _escapeSpeedThresholds = GetUnevenEscapeSpeedThresholds(colorSkew);
        
    }

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

    public abstract Color Apply(int x, int y);
}