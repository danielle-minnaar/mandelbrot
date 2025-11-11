using System.Numerics;
using Mandelbrot.ExtensionMethods;
using Mandelbrot.Helpers;
using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot.Generators;

/// <summary>
///     Class that calculates how quicky a point in the complex plane escapes a bound value.
/// </summary>
public class Calculator
{
    private IterationData? _prevCalc;
    private int _iterationFactor;
    private int _continuousBound;

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="Calculator"/> class.
    /// </summary>
    /// <param name="iterationFactor">
    ///     The value of the max calculated iterations is based on the previous
    ///     minimum iterations multiplied by this iteration factor.
    /// </param>
    /// <param name="continuousBound">
    ///     The boundary selected if the image is continous.
    /// </param>
    /// <remarks>
    ///     Higher values give greater image clarity, but slows down processing.
    /// </remarks>
    public Calculator(int iterationFactor = 200, int continuousBound = 2000)
    {
        _iterationFactor = iterationFactor;
        _continuousBound = continuousBound;
    }

    public IterationData GenerateIterationData(SpaceParam spaceParam, bool isContinuous)
    {
        var loopParam = new LoopParam
        {
            SpaceParam = spaceParam,
            Bound = isContinuous ? _continuousBound : 2,
            MaxCalculatedIterations = _prevCalc is null
                ? _iterationFactor
                : _prevCalc.MinIterations * _iterationFactor,
            IsContinuous = isContinuous
        };


        var builder = new IterationDataBuilder()
            .Initialize(loopParam)
            .ExecuteLoopedCalculation(ParallelLoop)
            .AddIterationMetaData()
            ;

        var result = builder.Build();
        _prevCalc = result;
        return result;
    }

    private CalcResult[,] ParallelLoop(LoopParam loopParam)
    {
        var spaceParam = loopParam.SpaceParam;
        var result = new CalcResult[spaceParam.XSize, spaceParam.YSize];
        var inputSpace = GenerateInputSpace(spaceParam);

        for (int x = 0; x < spaceParam.XSize; x++)
        {
            Parallel.For(0, spaceParam.YSize, y =>
            {
                var calcParam = loopParam.ToCalcParam(inputSpace[x, y]);
                result[x, y] = CalculateIteration(calcParam, 0, new Complex(0, 0));
            });
        }

        return result;
    }
    
    private CalcResult CalculateIteration(CalcParam calcParam, int currentIteration, Complex z)
    {
        if (z.Magnitude > calcParam.Bound)
        {
            var result = new CalcResult { Iterations = currentIteration };
            
            if (calcParam.isContinuous)
            {
                // This math is explained here:
                // https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set
                var logZn = Math.Log(z.Magnitude);
                var nu = Math.Log(logZn / Math.Log(calcParam.Bound)) / Math.Log(2);
                result.EscapeSpeed = currentIteration + 1d - nu;
            }
            
            return result;
        }

        if (currentIteration >= calcParam.MaxIterations)
        {
            var result = new CalcResult { Iterations = 0 };
            result.EscapeSpeed = calcParam.isContinuous ? 0d : null;

            return result;
        }

        z = z * z + calcParam.InputPoint;
        currentIteration++;
        return CalculateIteration(calcParam, currentIteration, z);
    }

    private static Complex[,] GenerateInputSpace(SpaceParam spaceParam)
    {
        var xMin = spaceParam.XMin;
        var xMax = spaceParam.XMax;
        var yMin = spaceParam.YMin;
        var yMax = spaceParam.YMax;
        var xSize = spaceParam.XSize;
        var ySize = spaceParam.YSize;

        var result = new Complex[xSize, ySize];
        
        var realStep = (xMax - xMin) / xSize;
        var imaginaryStep = (yMax - yMin) / ySize;

        var real = xMin;
        for (int x = 0; x < xSize; x++)
        {
            var imaginary = yMin;
            for (int y = 0; y < ySize; y++)
            {
                result[x, y] = new Complex(real, imaginary);
                imaginary += imaginaryStep;
            }
            
            real += realStep;
        }

        return result;
    }
}