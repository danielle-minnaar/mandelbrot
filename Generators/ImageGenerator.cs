using System.Drawing;
using Mandelbrot.Model;

namespace Mandelbrot.Generators;


/// <summary>
///     Generates Mandelbrot images using iteration counts or escape speeds.
/// </summary>
public class ImageGenerator
{
    private int bound;
    private ColorPicker colorPicker;


    /// <summary>
    ///     Initializes the ImageGenerator with a color picker and bound value.
    /// </summary
    public ImageGenerator(ColorPicker colorPicker, int bound)
    {
        this.colorPicker = colorPicker;
        this.bound = bound;
    }


    /// <summary>
    ///     Generates a Mandelbrot image using smooth coloring based on escape speed.
    /// </summary>
    /// <param name="input">
    ///     Complex space parameters.
    /// </param>
    /// <param name="maxIterations">
    ///     Maximum iterations for calculation.
    /// </param>
    /// <param name="isTiming">
    ///     If true, prints timing information.
    /// </param>
    /// <returns>
    ///     <see cref="BrotImage"/> containing the generated image and metadata.
    /// </returns
    public BrotImage GenerateSmooth(ComplexSpaceParameters input, int maxIterations, bool isTiming = false)
    {
        var calculator = new Calculator(input, bound, maxIterations);
        var result = calculator.CalculateEscapeSpeed(isTiming);

        var escapeSpeed = result.Item1;
        var iterations = result.Item2;

        var image = colorPicker.GetColorFromEscapeSpeed(escapeSpeed, isTiming);

        var queryable = iterations
            .Cast<int>()
            .Where(i => i != 0);

        var minIter = queryable.Min();
        var maxIter = queryable.Max();

        var resultImage = new BrotImage
        {
            complexSpaceParameters = input,
            Image = image,
            MaxIterations = maxIter,
            MinIterations = minIter
        };

        return resultImage;
    }

    public BrotImage Generate(ComplexSpaceParameters input, int maxIterations, bool isTiming = false)
    {

        var calculator = new Calculator(input, bound, maxIterations);
        var iterations = calculator.CalculateIterations(isTiming);
        var image = new Bitmap(input.XSize, input.YSize);

        var queryable = iterations.Cast<int>();

        var minIter = queryable.Where(i => i != 0).Min();
        var maxIter = queryable
            .Where(i => i != 0)
            .Order()
            .Skip((int)(queryable
                        .Where(i => i != 0)
                        .Count() * 0.9999) - 1)
            .First();


        var startTime = DateTime.Now;
        for (int x = 0; x < input.XSize; x++)
        {
            for (int y = 0; y < input.YSize; y++)
            {
                var color = colorPicker.GetColorFromIterations(
                    iterations[x, y],
                    minIter,
                    maxIter);
                image.SetPixel(x, y, color);
            }
        }
        var endTime = DateTime.Now;
        if (isTiming)
        {
            Console.WriteLine($"The coloring took this long: {endTime - startTime}");
        }

        var result = new BrotImage
        {
            complexSpaceParameters = input,
            Image = image,
            MaxIterations = maxIter,
            MinIterations = minIter
        };

        return result;
    }
}