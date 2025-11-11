using System.Drawing;
using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot.Generators;


/// <summary>
///     Generates Mandelbrot images using iteration counts or escape speeds.
/// </summary>
public class ImageGenerator
{
    private ColorPicker _colorPicker;
    private Calculator _calculator;


    /// <summary>
    ///     Initializes the ImageGenerator with a
    ///     color picker and a standard calculator.
    /// </summary>
    /// <param name="colorPicker">
    ///     The <see cref="ColorPicker"/> used to color the image.
    /// </param>
    public ImageGenerator(ColorPicker colorPicker)
    {
        _colorPicker = colorPicker;
        _calculator = new Calculator();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageGenerator"/> class
    ///     with specified color picker and a modified calculator.
    /// </summary>
    /// <param name="colorPicker">
    ///     The <see cref="ColorPicker"/> used to color the image.
    /// </param>
    /// <param name="initialIter">
    ///     The max iterations for the first image.
    /// </param>
    /// <param name="bound">
    ///     The boundary value.
    /// </param>
    /// <param name="iterFactor">
    ///     Used to dynamically determine the number of iterations
    ///     in image sequences.
    /// </param>
    public ImageGenerator(
        ColorPicker colorPicker,
        int initialIter = 200,
        int bound = 2000,
        int iterFactor = 200)
    {
        _colorPicker = colorPicker;
        _calculator = new Calculator(iterFactor, bound, initialIter);
    }


    /// <summary>
    ///     Generates a Mandelbrot image using smooth coloring based on escape speed.
    /// </summary>
    /// <param name="input">
    ///     Contains the size and resolution of the image.
    /// </param>
    /// <returns>
    ///     <see cref="BrotImage"/> containing the generated image and metadata.
    /// </returns>
    public BrotImage GenerateSmooth(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, true);

        var image = _colorPicker.GetColorFromEscapeSpeed(rawData);

        return image;
    }

    /// <summary>
    ///     Generates a Mandelbrot image with discrete color bands.
    /// </summary>
    /// <param name="input">
    ///     Contains the size and resolution of the image.
    /// </param>
    /// <returns>
    ///     <see cref="BrotImage"/> containing the generated image and metadata.
    /// </returns>
    public BrotImage Generate(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, false);

        var image = _colorPicker.GetColorFromIterations(rawData);

        return image;
    }

    /// <summary>
    ///     Get a genarator for a sequence of smooth Mandelbrot images.
    /// </summary>
    /// <param name="sequenceParam">
    ///     Parameters that specify the sequence.
    /// </param>
    /// <returns>
    ///     <see cref="SequenceGenerator"/> that generates the images.
    /// </returns>
    public SequenceGenerator GetSmoothSequence(SequenceParam sequenceParam)
    {
        var sequenceGenerator = new SequenceGenerator(sequenceParam, GenerateSmooth);
        return sequenceGenerator;
    }
    
    /// <summary>
    ///     Get a genarator for a sequence of Mandelbrot images with discrete color bands.
    /// </summary>
    /// <param name="sequenceParam">
    ///     Parameters that specify the sequence.
    /// </param>
    /// <returns>
    ///     <see cref="SequenceGenerator"/> that generates the images.
    /// </returns>
    public SequenceGenerator GetFastSequence(SequenceParam sequenceParam)
    {
        var sequenceGenerator = new SequenceGenerator(sequenceParam, Generate);
        return sequenceGenerator;
    }
}