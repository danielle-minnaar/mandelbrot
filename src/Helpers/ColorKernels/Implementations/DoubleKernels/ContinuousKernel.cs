using System.Drawing;
using Mandelbrot.src.Model;

namespace Mandelbrot.src.Helpers.ColorKernels.Implementations.DoubleKernels;

/// <summary>
///     The kernel that aplies continuous coloring.
/// </summary>
public class ContinuousKernel : DoubleKernelBase
{
    /// <summary>
    ///     The constructor.
    /// </summary>
    /// <param name="iterData">The data.</param>
    /// <param name="palette">The colors.</param>
    public ContinuousKernel(
        IterationData iterData,
        ColorPalette palette)
        : base(iterData, palette, ColorType.Continuous) {}
    
    /// <inheritdoc/>
    public override Color Apply(int x, int y)
    {
        var speed = _escapeSpeeds[x, y];

        if (speed == 0)
        {
            return Color.Black;
        }
        
        var (colorId, idFraction) = GetFractionalColorId(speed);
        
        var color1 = _palette.Colors[colorId];
        var color2 = _palette.Colors[Math.Min(colorId + 1, _palette.Colors.Length - 1)];

        return LinearInterpolate(color1, color2, idFraction);
    }

    private static Color LinearInterpolate(Color color1, Color color2, double fraction)
    {
        int R = (int)(color1.R + (color2.R - color1.R) * fraction);
        int G = (int)(color1.G + (color2.G - color1.G) * fraction);
        int B = (int)(color1.B + (color2.B - color1.B) * fraction);
        return Color.FromArgb(R, G, B);
    }
}