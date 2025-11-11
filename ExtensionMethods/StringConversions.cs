using Mandelbrot.Model;

namespace Mandelbrot.ExtensionMethods;

/// <summary>
///     Class that holds all of the custom ToMyString methods.
/// </summary>
public static class StringConversions
{
    /// <summary>
    ///     Turn the meta data of the <see cref="BrotImage"/> class
    ///     into human readable text.
    /// </summary>
    /// <param name="brotImage">
    ///     The supplied image.
    /// </param>
    /// <returns>
    ///     A <c>string</c> containing the meta data.
    /// </returns>
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