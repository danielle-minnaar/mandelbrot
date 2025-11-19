using System.Drawing;
using Mandelbrot.Model;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

/// <summary>
///     The kernel that applies dithered coloring.
/// </summary>
/// <remarks>
///     Best used for pixel art.
/// </remarks>
public class DitheredKernel : DoubleKernelBase
{
    private readonly double _ditherRatio;
    
    /// <summary>
    ///     The constructor.
    /// </summary>
    /// <param name="iterData">Raw data.</param>
    /// <param name="palette">The color palette.</param>
    /// <param name="colorSkew">The color skew.</param>
    /// <param name="ditherRatio">The ratio of a color band that is dithered</param>
    public DitheredKernel(
        IterationData iterData,
        Color[] palette,
        double colorSkew,
        double ditherRatio = 0.2d) : base(iterData, palette, colorSkew)
    {
        _ditherRatio = ditherRatio;
    }
    
    /// <inheritdoc/>
    public override Color Apply(int x, int y)
    {
        var speed = _escapeSpeeds[x, y];
        if (speed == 0)
        {
            return Color.Black;
        }

        var fractionalColorID = GetFractionalColorId(speed);
        var colorId = (int)fractionalColorID;
        var fraction = fractionalColorID - colorId;

        if ((x + y + colorId) % 2 == 0)
        {
            colorId -= fraction < _ditherRatio ? 1 : 0;
            colorId += fraction > (1 - _ditherRatio) ? 1 : 0;
            colorId = Math.Clamp(colorId, 0, _colorPalette.Length);
        }

        return _colorPalette[colorId];
    }
}