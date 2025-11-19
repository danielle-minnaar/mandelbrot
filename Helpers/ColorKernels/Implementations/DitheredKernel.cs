using System.Drawing;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

public class DitheredKernel : DoubleKernelBase
{
    private readonly double _ditherRatio;
    public DitheredKernel(
        double[,] escapeSpeeds,
        Color[] palette,
        double colorSkew,
        double ditherRatio = 0.2d) : base(escapeSpeeds, palette, colorSkew)
    {
        _ditherRatio = ditherRatio;
    }
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