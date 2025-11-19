using System.Numerics;
using Mandelbrot.ExtensionMethods;
using Mandelbrot.Helpers;
using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;

namespace Mandelbrot.Generators;

/// <summary>
///     Class that calculates how quicky a point in
///     the complex plane escapes a bound value.
/// </summary>
public class Calculator
{
    private IterationData? _prevCalc;
    private int _iterationFactor;
    private int _continuousBound;
    private int _initialIter;

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
    /// <param name="initialIter">
    ///     The max iterations for the first generated image.
    /// </param>
    /// <remarks>
    ///     Higher values give greater image clarity, but slows down processing.
    /// </remarks>
    public Calculator(
        int iterationFactor = 200,
        int continuousBound = 2000,
        int initialIter = 200)
    {
        _iterationFactor = iterationFactor;
        _continuousBound = continuousBound;
        _initialIter = initialIter;
    }

    /// <summary>
    ///     Generates the iteration data based on the supplied parameters.
    /// </summary>
    /// <param name="spaceParam">
    ///     The parameters that determine the scope of the data.
    /// </param>
    /// <param name="isContinuous">
    ///     If true also calculate the escape speed.
    ///     Slows down the calculation.
    /// </param>
    /// <returns></returns>
    public IterationData GenerateIterationData(SpaceParam spaceParam, bool isContinuous)
    {
        var loopParam = new LoopParam
        {
            SpaceParam = spaceParam,
            Bound = isContinuous ? _continuousBound : 2,
            MaxCalculatedIterations = _prevCalc is null
                ? _initialIter
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

    private (double[,]?, int[,]) ParallelLoop(LoopParam loopParam)
    {
        var spaceParam = loopParam.SpaceParam;
        var iterResult = new int[spaceParam.XSize, spaceParam.YSize];
        var speedResult = new double[spaceParam.XSize, spaceParam.YSize];
        var inputSpace = GenerateInputSpace(spaceParam);

        for (int x = 0; x < spaceParam.XSize; x++)
        {
            Parallel.For(0, spaceParam.YSize, y =>
            {
                var calcParam = loopParam.ToCalcParam(inputSpace[x, y]);
                var result = CalculateIteration(calcParam);
                iterResult[x, y] = result.Iterations;
                if (result.EscapeSpeed != null)
                {
                    speedResult[x, y] = (double)result.EscapeSpeed;
                }
            });
        }

        if (loopParam.IsContinuous)
        {
            return (speedResult, iterResult);
        }
        
        return (null, iterResult);
    }
    
    private CalcResult CalculateIteration(CalcParam calcParam)
    {
        var z = new Complex(0, 0);

        for (int i = 0; i < calcParam.MaxIterations; i++)
        {
            if (z.Magnitude > calcParam.Bound)
            {
                if (calcParam.isContinuous)
                {
                    // This math is explained here:
                    // https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set
                    var logZn = Math.Log(z.Magnitude);
                    var nu = Math.Log(logZn / Math.Log(calcParam.Bound)) / Math.Log(2);
                    return new CalcResult
                    {
                        Iterations = i,
                        EscapeSpeed = i + 1d - nu
                    };
                }

                return new CalcResult 
                { 
                    Iterations = i
                };
            }
            
            z = z * z + calcParam.InputPoint;
        }

        return new CalcResult
        {
            Iterations = 0,
            EscapeSpeed = calcParam.isContinuous ? 0d : null
        };
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