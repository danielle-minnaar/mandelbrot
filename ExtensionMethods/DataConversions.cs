using System.Drawing;
using System.Numerics;
using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot.ExtensionMethods;

public static class DataConversions
{
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

    public static BrotImage ToBrotImage(this IterationData iterationData, Bitmap image, TimeSpan colorTime)
    {
        return new BrotImage
        {
            Image = image,
            ColorTime = colorTime,
            IterationData = iterationData
        };
    }
}