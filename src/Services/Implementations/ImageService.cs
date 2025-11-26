using System.Drawing;
using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Helpers.ColorKernels;
using Mandelbrot.src.Helpers.ColorKernels.Implementations;
using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;
using Mandelbrot.src.Repositories;

namespace Mandelbrot.src.Services.Implementations;

/// <inheritdoc/>
public class ImageService : IImageService
{
    private readonly IPaletteRepository _palettes;
    private readonly ICalculationService _calculation;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageService"/> class.
    /// </summary>
    /// <param name="palettes">
    ///     The <see cref="IPaletteRepository"/>, supplied by DI.
    /// </param>
    /// <param name="calculation">
    ///     The <see cref="ICalculationService"/>, supplied by DI.
    /// </param>
    public ImageService(IPaletteRepository palettes, ICalculationService calculation)
    {
        _palettes = palettes;
        _calculation = calculation;
    }

    /// <inheritdoc/>
    public async Task<BrotImage> GetImage(SpaceParam inputParam)
    {
        var currentPalette = await _palettes.GetMostRecent();
        var colorType = ColorType.Continuous;

        var iterData = _calculation.Calculate(inputParam, colorType.IsContinuous());
        var kernel = KernelFactory.Create(colorType, iterData, currentPalette);
        var (image, colorTime) = Looper(kernel);
        return iterData.ToBrotImage(image, colorTime);
    }

    /// <summary>
    ///     Applies the kernel to the raw data it holds in a loop
    ///     to generate an image. Also times itself.
    /// </summary>
    /// <param name="kernel">
    ///     The supplied kernel that knows how to generate an image.
    /// </param>
    /// <returns>
    ///     A tuple containing the image and timing information.
    ///     (<see cref="Bitmap"/>, <see cref="TimeSpan"/>)
    /// </returns>
    private (Bitmap, TimeSpan) Looper(IColorKernel kernel)
    {
        var startTime = DateTime.Now;
        var (xSize, ySize) = kernel.GetImageSize();
        var image = new Bitmap(xSize, ySize);
        
        for (var x = 0; x < xSize; x++)
        for (var y = 0; y < ySize; y++)
        {
            image.SetPixel(x, y, kernel.Apply(x, y));
        }

        var colorTime = DateTime.Now - startTime;

        return (image, colorTime);
    }
}