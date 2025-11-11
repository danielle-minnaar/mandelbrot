using Mandelbrot.Model;

namespace Mandelbrot.ExtensionMethods;

public static class StringConversions
{
    public static string ToMyString(this BrotImage brotImage)
    {
        var spaceParam = brotImage.IterationData.SpaceParam;
        var iterData = brotImage.IterationData;

        return $@"
        This image is: {spaceParam.XSize} by {spaceParam.YSize} pixels.
        Its scale is: {spaceParam.XMax - spaceParam.XMin} by {spaceParam.YMax - spaceParam.YMin}.
        The maximum and minimum iterations are: {iterData.MaxIterations} and {iterData.MinIterations}.
        The number of black pixels is: {iterData.PointsInFractal}.
        Calculating took this long: {iterData.CalculationTime}.
        Coloring took this long: {brotImage.ColorTime}.";
    }
}