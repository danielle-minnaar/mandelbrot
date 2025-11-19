using System.Drawing.Imaging;
using System.Numerics;
using Mandelbrot.ExtensionMethods;
using Mandelbrot.Generators;
using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot;

public static class Tester
{
    public static void Test()
    {
        var colorPicker = new ColorPicker("ColdToWarm.png");
        var generator = new ImageGenerator(colorPicker);
        // var center = new Complex(-0.21756183674433, -1.11441769882846);
        var center = new Complex(-1, 0);
        // var size = 0.0000000001;
        var size = 4;
        var input = new SpaceParam(center, size, 1920/4, 1080/4);

        var image = generator.GenerateDithered(input);

        var filePath = Path.Combine("Storage/Images", "Image7.png");
        image.Image.Save(filePath, ImageFormat.Png);
    }

    public static void TestCalculationSpeed()
    {
        var colorPicker = new ColorPicker("ColdToWarm.png");
        var calculator = new Calculator();
        var images = new ImageGenerator(colorPicker, calculator);
        
        var inputParam = new SpaceParam(new Complex(-1, 0), 4, 1920, 1080);

        var imageList = new List<BrotImage>();
        
        calculator.GenerateIterationData(inputParam, false);
        imageList.Append(images.GenerateBanded(inputParam));
        calculator.GenerateIterationData(inputParam, false);
        imageList.Append(images.GenerateContinuous(inputParam));
        calculator.GenerateIterationData(inputParam, false);
        imageList.Append(images.GenerateDithered(inputParam));
        

        foreach (var image in imageList)
        {
            Console.WriteLine(image.ToMyString());
        }
        
    }

    public static void TestKernels()
    {
        var colorPicker = new ColorPicker("Alternator.png");
        var calculator = new Calculator(initialIter: 20000);
        var images = new ImageGenerator(colorPicker, calculator);

        var size = 0.0000000001;
        var center = new Complex(-0.21756183674433, -1.11441769882846);
        var inputParam = new SpaceParam(center, size, 1920, 1080);

        var image = images.GenerateContinuous(inputParam);

        Console.WriteLine(image.ToMyString());

        image.Image.Save("Storage/Images/KernelTest3.png");
    }

    public static void TestSequence()
    {
        var colorPicker = new ColorPicker("CtWMedium.png");
        var imageGenerator = new ImageGenerator(colorPicker);

        var sequenceParam = new SequenceParam
        {
            Center = new Complex(-0.21756183674433, -1.11441769882846),
            FinalSize = 0.00001,
            StartingSize = 10,
            ZoomFactor = 0.95,
            XResolution = 1920,
            YResolution = 1080
        };

        var sequenceGenerator = imageGenerator.GetContinuousSequence(sequenceParam);

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