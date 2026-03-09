using System.Drawing;
using Mandelbrot.src.Model;

namespace Mandelbrot.src.Helpers.ColorKernels.Implementations.DoubleKernels;

/// <summary>
///     The kernel that applies dithered coloring.
/// </summary>
/// <remarks>
///     Best used for pixel art.
/// </remarks>
/// <remarks>
///     The constructor.
/// </remarks>
/// <param name="iterData">Raw data.</param>
/// <param name="palette">The color palette.</param>
public class DitheredKernel(
    IterationData iterData,
    ColorPalette palette) : DoubleKernelBase(iterData, palette, ColorType.Dithered)
{

    /// <inheritdoc/>
    public override Color Apply(int x, int y)
    {
        var speed = _escapeSpeeds[x, y];
        if (speed == 0)
        {
            return Color.Black;
        }

        var (colorId, idFraction) = GetFractionalColorId(speed);

        if ((x + y + colorId) % 2 == 0)
        {
            colorId -= idFraction <= _palette.DitherRatio / 2 ? 1 : 0;
            colorId += 1 - idFraction < _palette.DitherRatio / 2 ? 1 : 0;
            colorId = Math.Clamp(colorId, 0, _palette.Colors.Length - 1);
        }

        return _palette.Colors[colorId];
    }
}