using System.Drawing;

namespace Mandelbrot.Helpers.ColorKernels;

public interface IColorKernel
{
    Color Apply(int x, int y);
}