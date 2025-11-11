using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot.Generators;

public class SequenceGenerator
{
    public bool HasNextImage => currentImageSize > _sequenceParam.FinalSize;

    private readonly SequenceParam _sequenceParam;
    private readonly Func<SpaceParam, BrotImage> _generatingFunction;


    private SpaceParam currentImageParam;
    private double currentImageSize;
    private BrotImage? currentImage;

    public SequenceGenerator(SequenceParam sequenceParam, Func<SpaceParam, BrotImage> generatingFunction)
    {
        _sequenceParam = sequenceParam;
        _generatingFunction = generatingFunction;

        currentImageSize = _sequenceParam.StartingSize;
        currentImageParam = new SpaceParam(
            _sequenceParam.Center,
            _sequenceParam.StartingSize,
            _sequenceParam.XResolution,
            _sequenceParam.YResolution);
    }

    public BrotImage Generate()
    {
        if (!HasNextImage)
        {
            throw new InvalidOperationException("No more images to generate.");
        }

        currentImage = _generatingFunction(currentImageParam);

        currentImageSize *= _sequenceParam.ZoomFactor;

        currentImageParam = new SpaceParam(
            _sequenceParam.Center,
            currentImageSize,
            _sequenceParam.XResolution,
            _sequenceParam.YResolution);

        return currentImage;
    }
}