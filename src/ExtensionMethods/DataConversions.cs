using System.Drawing;
using System.Numerics;
using Mandelbrot.src.Helpers.ColorKernels;
using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;

namespace Mandelbrot.src.ExtensionMethods;

/// <summary>
///     Class that holds extension methods for data conversions.
/// </summary>
public static class DataConversions
{
    /// <summary>
    ///     Get a <see cref="CalcParam"/> out of a <see cref="LoopParam"/>
    ///     based on a point defined by <see cref="LoopParam"/>.
    /// </summary>
    /// <param name="loopParam">
    ///     The supplied <see cref="LoopParam"/>.
    /// </param>
    /// <param name="inputPoint">
    ///     The point contained in the <see cref="LoopParam"/>.
    /// </param>
    /// <returns>
    ///     The parameters needed to calculate a single point.
    /// </returns>
    public static CalcParam ToCalcParam(this LoopParam loopParam, Complex inputPoint)
    {
        return new CalcParam
        {
            InputPoint = inputPoint,
            Bound = loopParam.Bound,
            MaxIterations = loopParam.MaxCalculatedIterations,
            isContinuous = loopParam.IsContinuous
        };
    }

    /// <summary>
    ///     Add a color image to the <see cref="IterationData"/>.
    /// </summary>
    /// <param name="iterationData">
    ///     The supplied <see cref="IterationData"/>.
    /// </param>
    /// <param name="image">
    ///     The color image.
    /// </param>
    /// <param name="colorTime">
    ///     The time it took to color the image.
    /// </param>
    /// <returns>
    ///     The completed <see cref="BrotImage"/>, including meta data.
    /// </returns>
    public static BrotImage ToBrotImage(this IterationData iterationData, Bitmap image, TimeSpan colorTime)
    {
        return new BrotImage
        {
            Image = image,
            ColorTime = colorTime,
            IterationData = iterationData
        };
    }

    /// <summary>
    ///     Determines if this <see cref="ColorType"/> is continuous or not.
    /// </summary>
    /// <remarks>
    ///     Continous <see cref="ColorType"/>s need to have escape speed calculated as well.
    /// </remarks>
    /// <param name="type">
    ///     The relevant <see cref="ColorType"/>.
    /// </param>
    /// <returns>
    ///     <c>true</c> if this <see cref="ColorType"/> is continuous,
    ///     otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool IsContinuous(this ColorType type)
    {
        var dict = new Dictionary<ColorType, bool>()
        {
            { ColorType.Banded, false },
            { ColorType.Continuous, true },
            { ColorType.Dithered, true}
        };

        var isSuccessful = dict.TryGetValue(type, out var result);
        if (!isSuccessful)
        {
            throw new ArgumentException($"No corresponding kernel registered for type: {type}");
        }

        return result;
    }
}