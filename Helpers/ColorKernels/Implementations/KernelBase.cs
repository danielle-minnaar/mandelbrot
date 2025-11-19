using System.Drawing;
using Mandelbrot.Model;

namespace Mandelbrot.Helpers.ColorKernels.Implementations;

/// <summary>
///     The base class that is inherited by all color kernels.
/// </summary>
public abstract class KernelBase : IColorKernel
{
    /// <summary>
    ///     The size of the raw data along x-coordinate/real component.
    /// </summary>
    protected int xSize;
    
    /// <summary>
    ///     The size of the raw data along y-coordinate/imaginary component.
    /// </summary>
    protected int ySize;

    /// <summary>
    ///     The color palette that is used to color the image.
    /// </summary>
    protected readonly Color[] _colorPalette;

    /// <summary>
    ///     Base constructor that all color kernels inherit.
    /// </summary>
    /// <param name="iterData">
    ///     The raw data that the kernel is applied to.
    /// </param>
    /// <param name="colorPalete">
    ///     The color palette that is used to color the image.
    /// </param>
    protected KernelBase(IterationData iterData, Color[] colorPalete)
    {
        xSize = iterData.SpaceParam.XSize;
        ySize = iterData.SpaceParam.YSize;
        _colorPalette = colorPalete;
    }
    
    /// <inheritdoc/>
    public (int, int) GetImageSize()
    {
        return (xSize, ySize);
    }
    
    /// <inheritdoc/>
    public abstract Color Apply(int x, int y);    
}