using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot.Generators;

/// <summary>
///     A generator that can be used to make zoom-sequences of images.
/// </summary>
public class SequenceGenerator
{
    /// <summary>
    ///     If true at least one another image can be generated
    ///     before the end of the sequence is reached.
    /// </summary>
    public bool HasNextImage => currentImageSize > _sequenceParam.FinalSize;

    private readonly SequenceParam _sequenceParam;
    private readonly Func<SpaceParam, BrotImage> _generatingFunction;


    private SpaceParam currentImageParam;
    private double currentImageSize;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SequenceGenerator"/> class.
    /// </summary>
    /// <param name="sequenceParam">
    ///     The parameters that determine the zoom and image parameters.
    /// </param>
    /// <param name="generatingFunction">
    ///     The function that is used to generate the images.
    /// </param>
    public SequenceGenerator(
        SequenceParam sequenceParam,
        Func<SpaceParam, BrotImage> generatingFunction)
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

    /// <summary>
    ///     The function that can be called repeatedly
    ///     to get the next image in the sequence.
    /// </summary>
    /// <returns>
    ///     The next <see cref="BrotImage"/> in the sequence.
    /// </returns>
    /// <exception cref="InvalidOperationException"></exception>
    public BrotImage Generate()
    {
        if (!HasNextImage)
        {
            throw new InvalidOperationException("No more images to generate.");
        }

        var currentImage = _generatingFunction(currentImageParam);

        currentImageSize *= _sequenceParam.ZoomFactor;

        currentImageParam = new SpaceParam(
            _sequenceParam.Center,
            currentImageSize,
            _sequenceParam.XResolution,
            _sequenceParam.YResolution);

        return currentImage;
    }
}