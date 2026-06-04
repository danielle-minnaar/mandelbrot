using Mandelbrot.src.Model;
using SkiaSharp;

namespace Mandelbrot.src.Helpers.ColorKernels.Implementations.DoubleKernels;

/// <summary>
///     The kernel that aplies continuous coloring.
/// </summary>
/// <remarks>
///     The constructor.
/// </remarks>
/// <param name="iterData">The data.</param>
/// <param name="palette">The colors.</param>
public class ContinuousKernel(
    IterationData iterData,
    ColorPalette palette) : DoubleKernelBase(iterData, palette, ColorType.Continuous)
{

    /// <inheritdoc/>
    public override SKColor Apply(int x, int y)
    {
        var speed = _escapeSpeeds[x, y];

        if (speed == 0)
        {
            return SKColor.FromHsv(h: 0, s: 0, v: 0);
        }
        
        var (colorId, idFraction) = GetFractionalColorId(speed);
        
        var color1 = _palette.Colors[colorId];
        var color2 = _palette.Colors[Math.Min(colorId + 1, _palette.Colors.Length - 1)];

        return LinearInterpolate(color1, color2, idFraction);
    }

    private static SKColor LinearInterpolate(SKColor color1, SKColor color2, double fraction)
    {
        int R = (int)(color1.Red + (color2.Red - color1.Red) * fraction);
        int G = (int)(color1.Green + (color2.Green - color1.Green) * fraction);
        int B = (int)(color1.Blue + (color2.Blue - color1.Blue) * fraction);
        return new SKColor((byte)R, (byte)G, (byte)B);
    }
}