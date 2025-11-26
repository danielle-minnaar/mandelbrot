using Mandelbrot.src.Helpers.ColorKernels.Implementations.DoubleKernels;
using Mandelbrot.src.Helpers.ColorKernels.Implementations.IntegerKernels;
using Mandelbrot.src.Model;

namespace Mandelbrot.src.Helpers.ColorKernels.Implementations;

/// <summary>
///     Static class used to instantiate color kernels.
/// </summary>
public static class KernelFactory
{
    private static readonly
    Dictionary<ColorType, Func<IterationData, ColorPalette, IColorKernel>> _factories =
        new()
        {
            { ColorType.Banded, (data, palette) => new BandedKernel(data, palette) },
            { ColorType.Continuous, (data, palette) => new ContinuousKernel(data, palette) },
            { ColorType.Dithered, (data, palette) => new DitheredKernel(data, palette) }
        };
    
    /// <summary>
    ///     Initializes a new instance of a class inheriting from
    ///     <see cref="IColorKernel"/>.
    /// </summary>
    /// <param name="type">
    ///     The <see cref="ColorType"/> type of coloring used be the new kernel.
    /// </param>
    /// <param name="iterData">
    ///     The raw <see cref="IterationData"/> data that the kernel works on.
    /// </param>
    /// <param name="palette">
    ///     The <see cref="ColorPalette"/> that the image will have.
    /// </param>
    /// <returns>
    ///     The new <see cref="IColorKernel"/> instance.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public static IColorKernel Create(
        ColorType type,
        IterationData iterData,
        ColorPalette palette)
    {
        var result = _factories.GetValueOrDefault(type)
            ?? throw new ArgumentException($"No corresponding kernel for type: {type}");
        return result(iterData, palette);
    }
}