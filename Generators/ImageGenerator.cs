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
    ///     color picker and a calculator.
    /// </summary>
    /// <param name="colorPicker">
    ///     The <see cref="ColorPicker"/> used to color the image.
    /// </param>
    /// <param name="calculator">
    ///     The <see cref="Calculator"/> used to calculate the raw data.
    ///     If left empty it will use the standard calculator.
    /// </param>
    public ImageGenerator(ColorPicker colorPicker, Calculator? calculator = null)
    {
        _calculator = calculator is null ? new Calculator() : calculator;
        _colorPicker = colorPicker;
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
    public BrotImage GenerateContinuous(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, true);

        var image = _colorPicker.GetContinuousColor(rawData);

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
    public BrotImage GenerateBanded(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, false);

        var image = _colorPicker.GetBandedColor(rawData);

        return image;
    }

    public BrotImage GenerateDithered(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, true);

        var image = _colorPicker.GetDitheredColor(rawData);

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
    public SequenceGenerator GetContinuousSequence(SequenceParam sequenceParam)
    {
        var sequenceGenerator = new SequenceGenerator(sequenceParam, GenerateContinuous);
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
    public SequenceGenerator GetBandedSequence(SequenceParam sequenceParam)
    {
        var sequenceGenerator = new SequenceGenerator(sequenceParam, GenerateBanded);
        return sequenceGenerator;
    }
}