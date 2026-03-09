using System.Numerics;
using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Helpers;
using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;
using Mandelbrot.src.Singletons;
using Microsoft.Extensions.Options;

namespace Mandelbrot.src.Services.Implementations;

/// <inheritdoc/>
/// <summary>
///     Initializes a new instance of the <see cref="CalculationService"/> class.
/// </summary>
/// <param name="options">
///     Contains the <see cref="AppSettings"/> that configures calculations. Supplied by DI.
/// </param>
public class CalculationService(IOptions<AppSettings> options) : ICalculationService
{
    private int iterFactor = options.Value.IterationFactor;
    private int maxIter = options.Value.InitialIterations;
    private double progress = 0;

    /// <inheritdoc/>
    public IterationData Calculate(SpaceParam spaceParam, bool isContinuous)
    {
        var loopParam = new LoopParam
        {
            SpaceParam = spaceParam,
            Bound = isContinuous ? 2000 : 2,
            MaxCalculatedIterations = maxIter,
            IsContinuous = isContinuous
        };


        var builder = new IterationDataBuilder()
            .Initialize(loopParam)
            .ExecuteLoopedCalculation(ParallelLoop)
            .AddIterationMetaData()
            ;

        var result = builder.Build();
        maxIter = result.MinIterations * iterFactor;
        return result;
    }

    /// <inheritdoc/>
    public double GetProgress()
    {
        return progress;
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

            progress = (1d + x) / spaceParam.XSize;
        }

        progress = 0;

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
        var unpacker = (SpaceParam s) =>
            (s.XMin, s.XMax, s.YMin, s.YMax, s.XSize, s.YSize);
        var (xMin, xMax, yMin, yMax, xSize, ySize) = unpacker(spaceParam);

        var result = new Complex[xSize, ySize];
        
        var realStep = (xMax - xMin) / xSize;
        var imaginaryStep = (yMax - yMin) / ySize;

        var real = xMin;
        for (int x = 0; x < xSize; x++)
        {
            var imaginary = yMin;
            for (int y = ySize - 1; y >= 0; y--)
            {
                result[x, y] = new Complex(real, imaginary);
                imaginary += imaginaryStep;
            }
            
            real += realStep;
        }

        return result;
    }
}