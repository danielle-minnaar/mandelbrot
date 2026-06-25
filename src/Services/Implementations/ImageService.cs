using Mandelbrot.src.Exceptions;
using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Helpers.ColorKernels;
using Mandelbrot.src.Helpers.ColorKernels.Implementations;
using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;
using SkiaSharp;

namespace Mandelbrot.src.Services.Implementations;

/// <inheritdoc/>
/// <summary>
///     Initializes a new instance of the <see cref="ImageService"/> class.
/// </summary>
/// <param name="palettes">
///     The <see cref="IPaletteService"/>, supplied by DI.
/// </param>
/// <param name="calculation">
///     The <see cref="ICalculationService"/>, supplied by DI.
/// </param>
public class ImageService(IPaletteService palettes, ICalculationService calculation) : IImageService
{
    private readonly IPaletteService _palettes = palettes;
    private readonly ICalculationService _calculation = calculation;

    private bool isBusy = false;
    private BrotImage? currentImage;
    private Dictionary<Guid, BrotImage> images = new Dictionary<Guid, BrotImage>();

    /// <inheritdoc/>
    public Guid GenerateImage(SpaceParam inputParam)
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

            var imGuid = Guid.NewGuid();
            
            var isCon = _palettes.ColorType.IsContinuous();
            var iterData = _calculation.Calculate(inputParam, isCon);
            currentImage = GetColorImage(iterData);

            images.Add(imGuid, currentImage);
            
            isBusy = false;

            return imGuid;
        }
        catch (Exception)
        {
            isBusy = false;
            throw;
        }
        
    }

    /// <inheritdoc/>
    public BrotImage GetImage(Guid imguid)
    {
        var result = images.GetValueOrDefault(imguid);
        if (result == null)
        {
            var m = $"Could not find an image with Guid: {imguid}";
            throw new NullReferenceException(m);
        }
        return result;
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
    public BrotImage GetRecoloredImage()
    {
        var iterData = GetImage().IterationData;
        return GetColorImage(iterData);
    }

    /// <inheritdoc/>
    public double GetProgress()
    {
        return _calculation.GetProgress();
    }

    private BrotImage GetColorImage(IterationData iterData)
    {
        var colorType = _palettes.ColorType;
        var currentPalette = _palettes.CurrentPalette;

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
    ///     (<see cref="SKBitmap"/>, <see cref="TimeSpan"/>)
    /// </returns>
    private (SKBitmap, TimeSpan) Looper(IColorKernel kernel)
    {
        var startTime = DateTime.Now;
        var (xSize, ySize) = kernel.GetImageSize();
        var image = new SKBitmap(xSize, ySize);
        
        for (var x = 0; x < xSize; x++)
        for (var y = 0; y < ySize; y++)
        {
            image.SetPixel(x, y, kernel.Apply(x, y));
        }

        var colorTime = DateTime.Now - startTime;

        return (image, colorTime);
    }
}