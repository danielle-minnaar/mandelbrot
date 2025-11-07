using System.Numerics;

namespace Mandelbrot;

public static class Tester
{
    public static void Test()
    {
        var colorPicker = new ColorPicker("BigPallette2.png");
        var generator = new ImageGenerator(colorPicker, 2000);
        var center = new Complex(-0.21756183674433, -1.11441769882846);
        // var center = new Complex(-1, 0);
        // var size = 0.0000000001;
        var size = 0.0001;
        var input = new ComplexSpaceParameters(center, size, 1920, 1080);

        var image = generator.GenerateSmooth(input, 4000, true);

        var message = $"The maximum iterations were: {image.MaxIterations}";
        message += $", and the minimum iterations were: {image.MinIterations}";
        Console.WriteLine(message);

        var filePath = Path.Combine("Storage/Images", "Image1.png");
        image.Image.Save(filePath, ImageFormat.Png);
    }

    public static void TestSequence()
    {
        var colorPicker = new ColorPicker("BigPallette2.png");
        var center = new Complex(-0.21756183674433, -1.11441769882846);
        var generator = new ImageGenerator(colorPicker, 2000);

        var size = 10d;
        var zoomFactor = 0.9d;
        var iterFactor = 200;
        var maxIters = iterFactor;

        var baseFileName = "Storage/Images/ZoomLevel_";

        var imageNumber = 0;

        while (size > 0.0001)
        {
            var input = new ComplexSpaceParameters(center, size, 1920, 1080);
            BrotImage image;
            if (imageNumber++ % 10 == 0)
            {
                image = generator.GenerateSmooth(input, maxIters, true);
                Console.WriteLine($"The image size is now: {size}");
                var message = $@"
                    The maximum iterations were: {image.MaxIterations}
                    , and the minimum iterations were: {image.MinIterations}";
                Console.WriteLine(message);
            }
            else
            {
                image = generator.GenerateSmooth(input, maxIters, false);
            }
            
            image.Image.Save($"{baseFileName}{imageNumber}.png", ImageFormat.Png);

            size *= zoomFactor;
            maxIters = ((int)image.MinIterations + 1) * iterFactor;
            imageNumber++;
        }
        
    }
}