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
    ///     Initializes the ImageGenerator with a color picker and a calculator.
    /// </summary
    public ImageGenerator(ColorPicker colorPicker)
    {
        _colorPicker = colorPicker;
        _calculator = new Calculator();
    }


    /// <summary>
    ///     Generates a Mandelbrot image using smooth coloring based on escape speed.
    /// </summary>
    /// <param name="input">
    ///     Complex space parameters.
    /// </param>
    /// <returns>
    ///     <see cref="BrotImage"/> containing the generated image and metadata.
    /// </returns
    public BrotImage GenerateSmooth(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, true);

        var image = _colorPicker.GetColorFromEscapeSpeed(rawData);

        return image;
    }

    public BrotImage Generate(SpaceParam input)
    {
        var rawData = _calculator.GenerateIterationData(input, false);

        var image = _colorPicker.GetColorFromIterations(rawData);

        return image;
    }

    public SequenceGenerator GetSmoothSequence(SequenceParam sequenceParam)
    {
        var sequenceGenerator = new SequenceGenerator(sequenceParam, GenerateSmooth);
        return sequenceGenerator;
    }
    
    public SequenceGenerator GetFastSequence(SequenceParam sequenceParam)
    {
        var sequenceGenerator = new SequenceGenerator(sequenceParam, Generate);
        return sequenceGenerator;
    }
}