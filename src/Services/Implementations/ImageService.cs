using System.Drawing;
using System.Threading.Tasks;
using Mandelbrot.src.Exceptions;
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

    private bool isBusy = false;
    private BrotImage? currentImage;

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
    public async Task GenerateImage(SpaceParam inputParam)
    {
        if (isBusy)
        {
            var progressPercent =(int) GetProgress() * 100;
            var m = "Can't execute two generations at the same time";
            m += $", current generartion is at: {progressPercent}%";
            throw new InOperationException(m);
        }

        try
        {
            isBusy = true;
            
            var isCon = ColorType.Continuous.IsContinuous();
            var iterData = _calculation.Calculate(inputParam, isCon);
            currentImage = await GetColorImage(iterData);
            
            isBusy = false;
        }
        catch (Exception)
        {
            isBusy = false;
            throw;
        }
        
    }

    /// <inheritdoc/>
    public BrotImage GetImage()
    {
        if (currentImage is null)
        {
            var m = "An image has not been generated yet";
            var progressPercent =(int) _calculation.GetProgress() * 100;
            m += _calculation.GetProgress() == 0
                ? " and an image is not currently being generated."
                : $", but an image is currently being generated. Calulation is at: {progressPercent}%";

            throw new NullReferenceException(m);
        }

        return currentImage;
    }

    /// <inheritdoc/>
    public async Task<BrotImage> GetRecoloredImage()
    {
        var iterData = GetImage().IterationData;
        return await GetColorImage(iterData);
    }

    /// <inheritdoc/>
    public double GetProgress()
    {
        return _calculation.GetProgress();
    }

    private async Task<BrotImage> GetColorImage(IterationData iterData)
    {
        var colorType = ColorType.Continuous;
        var currentPalette = await _palettes.GetMostRecent();

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