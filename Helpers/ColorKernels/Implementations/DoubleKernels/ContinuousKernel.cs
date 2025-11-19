using System.Drawing;
using Mandelbrot.Model;

namespace Mandelbrot.Helpers.ColorKernels.Implementations.DoubleKernels;

/// <summary>
///     The kernel that aplies continuous coloring.
/// </summary>
public class ContinuousKernel : DoubleKernelBase
{
    /// <summary>
    ///     The constructor.
    /// </summary>
    /// <param name="iterData">The data.</param>
    /// <param name="colorPalette">The colors.</param>
    /// <param name="colorSkew">The color skew.</param>
    public ContinuousKernel(
        IterationData iterData,
        Color[] colorPalette,
        double colorSkew) : base(iterData, colorPalette, colorSkew) {}
    
    /// <inheritdoc/>
    public override Color Apply(int x, int y)
    {
        var speed = _escapeSpeeds[x, y];

        if (speed == 0)
        {
            return Color.Black;
        }
        
        var fractionalColorID = GetFractionalColorId(speed);
        var colorId = (int)Math.Floor(fractionalColorID);
        var fraction = fractionalColorID - colorId;
        
        var color1 = _colorPalette[colorId];
        var color2 = _colorPalette[Math.Min(colorId + 1, _colorPalette.Length - 1)];

        return LinearInterpolate(color1, color2, fraction);
    }

    private static Color LinearInterpolate(Color color1, Color color2, double fraction)
    {
        int R = (int)(color1.R + (color2.R - color1.R) * fraction);
        int G = (int)(color1.G + (color2.G - color1.G) * fraction);
        int B = (int)(color1.B + (color2.B - color1.B) * fraction);
        return Color.FromArgb(R, G, B);
    }
}