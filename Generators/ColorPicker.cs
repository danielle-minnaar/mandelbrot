using System.Drawing;

namespace Mandelbrot.Generators;

/// <summary>
///     Class that is used to turn raw Mandelbrot data into colors.
/// </summary>
public class ColorPicker
{
    private List<Color> colors = new List<Color>();
    private static string pallettesFolder = "Storage/Pallettes/";

    private List<double> _escapeSpeedThresholds = new List<double>();

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="ColorPicker"/> class.
    /// </summary>
    /// <param name="palletteName">
    ///     The name of the pallette file.
    ///     The file should be x by 1 in size.
    /// </param>
    public ColorPicker(string palletteName)
    {
        var path = Path.Combine(
            pallettesFolder,
            palletteName);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Couldn't find this pallette: {path}");
        }

        var pallette = new Bitmap(path);

        for (int i = 0; i < pallette.Width; i++)
        {
            var color = pallette.GetPixel(i, 0);
            colors.Add(color);
        }
    }

    public Bitmap GetColorFromEscapeSpeed(double[,] escapeSpeeds, bool isTiming = false)
    {
        var startTime = DateTime.Now;
        
        SetEscapeSpeedThresholds(escapeSpeeds);
        
        var image = new Bitmap(escapeSpeeds.GetLength(0), escapeSpeeds.GetLength(1));

        for (int x = 0; x < escapeSpeeds.GetLength(0); x++)
        {
            for (int y = 0; y < escapeSpeeds.GetLength(1); y++)
            {
                var color = GetColorFromEscapeSpeed(escapeSpeeds[x, y]);
                image.SetPixel(x, y, color);
            }
        }

        if (isTiming) Console.WriteLine($"The coloring took this long: {DateTime.Now - startTime}");
        
        return image;
    }

    private void SetEscapeSpeedThresholds(double[,] escapeSpeeds)
    {
        var numThresholds = colors.Count();

        var queryable = escapeSpeeds.Cast<double>();
        var count = queryable.Where(i => i != 0).Count();
        var skipSize = count / numThresholds;

        var max = queryable.Max();
        queryable = queryable
            .Where(i => i != 0)
            .Order()
            .Where((item, index) => index % skipSize == 0)
            .SkipLast(1)
            ;
        var result = queryable.ToList();
        result.Add(max);
        _escapeSpeedThresholds = result;
    }

    private Color GetColorFromEscapeSpeed(double escapeSpeed)
    {
        if (escapeSpeed == 0)
        {
            return Color.Black;
        }

        var colorId = 0;
        var lowerValue = _escapeSpeedThresholds[0];
        var higherValue = _escapeSpeedThresholds[_escapeSpeedThresholds.Count() - 1];
        for (int i = 0; i < _escapeSpeedThresholds.Count(); i++)
        {
            var threshold = _escapeSpeedThresholds[i];
            if (threshold <= escapeSpeed && threshold > lowerValue)
            {
                lowerValue = threshold;
                colorId = Math.Min(i, colors.Count() - 1);
            }
            else if (threshold > escapeSpeed && threshold < higherValue)
            {
                higherValue = threshold;
            }
        }

        var fraction = (escapeSpeed - lowerValue) / (higherValue - lowerValue);
        var color1 = colors[colorId];
        var color2 = colors[Math.Min(colorId + 1, colors.Count() - 1)];

        return InterpolateColors(color1, color2, fraction);
    }
    
    private static Color InterpolateColors(Color color1, Color color2, double fraction)
    {
        if (fraction < 0 || fraction > 1)
        {
            throw new ArgumentException($"This is not correct: {fraction}");
        }
        int R = (int)(
            Math.Min(color1.R, color2.R) +
            Math.Abs(color1.R - color2.R) * fraction);
        int G = (int)(
            Math.Min(color1.G, color2.G) +
            Math.Abs(color1.G - color2.G) * fraction);
        int B = (int)(
            Math.Min(color1.B, color2.B) +
            Math.Abs(color1.B - color2.B) * fraction);
        return Color.FromArgb(R, G, B);
    }
    
    /// <summary>
    ///     Get the appropriate color for a point in the image.
    /// </summary>
    /// <param name="iterations">
    ///     The number of iterations to escape the bound value for the point.
    /// </param>
    /// <param name="minIterations">
    ///     The minimum number of iterations of all points in the image
    ///     that lie outside the fractal.
    /// </param>
    /// <param name="maxIterations">
    ///     Tha maximum number of iterations of all points in the image
    ///     that lie outside the fractal.
    /// </param>
    /// <returns>
    ///     The <c>Color</c> that matches the number of iterations.
    /// </returns>
    public Color GetColorFromIterations(Int128 iterations,
                                        Int128 minIterations,
                                        Int128 maxIterations)
    {

        if (iterations == 0)
        {
            return Color.Black;
        }
        
        // Scale iterations from 0 to 1
        var enumerator = (double)(iterations - minIterations + 1);
        var divisor = (double)(maxIterations - minIterations);
        var adjustedIterations = (double)enumerator / (double)divisor;

        // Scale from 0 to three times the number of colors
        adjustedIterations = adjustedIterations * colors.Count * 3;


        var slope = 2d / colors.Count;
        divisor = 1 + Math.Pow(Math.E, -1 * slope * adjustedIterations);

        var colorIndex = (2 / divisor - 1) * colors.Count;
        colorIndex = Math.Min(colorIndex, colors.Count - 1);
        
        // var colorIndex = Math.Min((int)(iterations - minIterations), colors.Count - 1);

        return colors[(int)colorIndex];
    }
}