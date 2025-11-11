using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using Mandelbrot.ExtensionMethods;
using Mandelbrot.Generators;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot;

public static class Tester
{
    public static void Test()
    {
        var colorPicker = new ColorPicker("CtWMedium.png");
        var generator = new ImageGenerator(colorPicker);
        // var center = new Complex(-0.21756183674433, -1.11441769882846);
        var center = new Complex(-1, 0);
        // var size = 0.0000000001;
        var size = 4;
        var input = new SpaceParam(center, size, 1920, 1080);

        var image = generator.GenerateSmooth(input);

        var filePath = Path.Combine("Storage/Images", "Image5.png");
        image.Image.Save(filePath, ImageFormat.Png);
    }

    public static void TestSequence()
    {
        var colorPicker = new ColorPicker("CtWMedium.png");
        var imageGenerator = new ImageGenerator(colorPicker);

        var sequenceParam = new SequenceParam
        {
            Center = new Complex(-0.21756183674433, -1.11441769882846),
            FinalSize = 0.001,
            StartingSize = 10,
            ZoomFactor = 0.3,
            XResolution = 1920,
            YResolution = 1080
        };

        var sequenceGenerator = imageGenerator.GetSmoothSequence(sequenceParam);

        var baseFileName = "Storage/Images/ZoomLevel_";

        var imageNumber = 0;

        while (sequenceGenerator.HasNextImage)
        {
            var image = sequenceGenerator.Generate();

            Console.WriteLine($"Image number: {imageNumber}");
            Console.WriteLine(image.ToMyString());

            image.Image.Save($"{baseFileName}{imageNumber}.png", ImageFormat.Png);

            imageNumber++;            
        }
        
    }
}