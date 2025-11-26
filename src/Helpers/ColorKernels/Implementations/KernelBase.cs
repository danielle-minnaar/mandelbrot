using System.Drawing;
using Mandelbrot.src.Model;

namespace Mandelbrot.src.Helpers.ColorKernels.Implementations;

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
    protected readonly ColorPalette _palette;

    /// <inheritdoc/>
    public ColorType Type { get; init; }

    /// <summary>
    ///     Base constructor that all color kernels inherit.
    /// </summary>
    /// <param name="iterData">
    ///     The raw data that the kernel is applied to.
    /// </param>
    /// <param name="colorPalette">
    ///     The color palette that is used to color the image.
    /// </param>
    /// <param name="type">
    ///     Contains the type of coloring of this kernel.
    /// </param>
    protected KernelBase(IterationData iterData, ColorPalette colorPalette, ColorType type)
    {
        Type = type;
        xSize = iterData.SpaceParam.XSize;
        ySize = iterData.SpaceParam.YSize;
        _palette = colorPalette;
    }
    
    /// <inheritdoc/>
    public (int, int) GetImageSize()
    {
        return (xSize, ySize);
    }
    
    /// <inheritdoc/>
    public abstract Color Apply(int x, int y);    
}