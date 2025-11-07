using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Mandelbrot.ExtensionMethods;
using Mandelbrot.Helpers;
using Mandelbrot.Model;
using Microsoft.VisualBasic;

namespace Mandelbrot.Generators;

/// <summary>
///     Class that calculates how quicky a point in the complex plane escapes a bound value.
/// </summary>
public class Calculator
{
    private IterationData? previousCalculation;
    private int iterationFactor = 200;
    private int continuousBound = 2000;
    /// <summary>
    ///     The collection of points in complex space on which calculations will be made.
    /// </summary>
    private Complex[,] inputSpace;

    /// <summary>
    ///     The value beyond which there is no return to the fractal
    /// </summary>
    private int _bound;

    /// <summary>
    ///     The maximum number of iterations before a point is
    ///     decided to lie inside the fractal.
    /// </summary>
    private int _maxIterations;
    

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="Calculator"/> class.
    /// </summary>
    /// <param name="inputSpaceParameters">
    ///     This is used to generate the input space.
    /// </param>
    /// <param name="maxIterations">
    ///     The maximum number of iterations before a point is
    ///     decided to lie inside the fractal.
    /// </param>
    /// <param name="bound">
    ///     The value beyond which there is no return to the fractal
    /// </param>
    public Calculator(ComplexSpaceParameters inputSpaceParameters, int bound, int maxIterations)
    {
        inputSpace = GenerateInputSpace(inputSpaceParameters);
        _bound = bound;
        _maxIterations = maxIterations;
    }

    public IterationData Calculate(ComplexSpaceParameters inputSpaceParameters, bool isContinuous)
    {
        var bound = isContinuous ? continuousBound : 2;
        var maxIterations =
                previousCalculation is null
                ? 200
                : previousCalculation.MinIterations * iterationFactor;

        Func<CalculationResult> calculation = isContinuous
            ? (() => CalculateEscapeSpeeds(inputSpaceParameters, bound, maxIterations))
            : (() => CalculateIterations(inputSpaceParameters, bound, maxIterations));

        var builder = new IterationDataBuilder()
            .Initialize(inputSpaceParameters, maxIterations, bound)
            .AddCalculationResult(calculation())
            .AddIterationMetaData()
            ;

        return builder.Build();
    }
    
    private CalculationResult CalculateEscapeSpeeds(
        ComplexSpaceParameters inputParam,
        int bound,
        int maxIterations)
    {
        var resultIterations = new int[inputParam.XSize, inputParam.YSize];
        var resultEscapeSpeeds = new double[inputParam.XSize, inputParam.YSize];
        var inputSpace = GenerateInputSpace(inputParam);

        for (int x = 0; x < inputParam.XSize; x++)
        {
            Parallel.For(0, inputParam.YSize, i =>
            {
                var result = CalculateEscapeSpeed(inputSpace[x, i], bound, maxIterations);
                resultEscapeSpeeds[x, i] = result.Item1;
                resultIterations[x, i] = result.Item2;
            });
        }

        return new CalculationResult
        {
            Iterations = resultIterations,
            EscapeSpeeds = resultEscapeSpeeds
        };
    }

    private CalculationResult CalculateIterations(
        ComplexSpaceParameters inputParam,
        int bound,
        int maxIterations)
    {
        var inputSpace = GenerateInputSpace(inputParam);
        var result = new int[inputParam.XSize, inputParam.YSize];

        for (int x = 0; x < inputParam.XSize; x++)
        {
            Parallel.For(0, inputParam.YSize, i =>
            {
                result[x, i] = CalculateIteration(inputSpace[x, i], bound, maxIterations);
            });
        }

        return new CalculationResult
        {
            Iterations = result
        };
    }

    private int CalculateIteration(Complex input, int bound, int maxIterations)
    {
        var z = new Complex(0, 0);
        for (int i = 0; i < maxIterations; i++)
        {
            if (z.Magnitude > bound)
            {
                return i;
            }
            z = z * z + input;
        }
        return 0;
    }

    /// <summary>
    ///     For every point in the input space calculate
    ///     the number of iterations to escape the bound value.
    /// </summary>
    /// <param name="isTiming">
    ///     If true print how long it took to calculate the number of iterations.
    /// </param>
    /// <returns>
    ///     A collecton of points inside an <c>Int128[,]</c> that contains
    ///     the number of iterations required to escape the bound value.
    ///     A value of 0 indicates that the point lies inside of the fractal.
    /// </returns>
    

    // public int[,] CalculateIterations(bool isTiming = false)
    // {
    //     var startTime = DateTime.Now;
    //     var result = new int[inputSpace.GetLength(0), inputSpace.GetLength(1)];
    //     for (int x = 0; x < inputSpace.GetLength(0); x++)
    //     {
    //         Func<int, int> func = i =>
    //         {
    //             result[x, i] = CalculateIteration(inputSpace[x, i]);
    //             return i;
    //         };
    //         Parallel.For(0, inputSpace.GetLength(1), i => func(i));
    //     }
    //     var endTime = DateTime.Now;
    //     if (isTiming)
    //     {
    //         Console.WriteLine($"Calculating iterations took this long: {endTime - startTime}");
    //     }
    //     return result;
    // }
    
    /// <summary>
    ///     For every point in the input space calculate
    ///     the escape speed to escape the bound value.
    /// </summary>
    /// <param name="isTiming">
    ///     If true print how long it took to calculate the number of iterations.
    /// </param>
    /// <returns>
    ///     A tuple that firstly contains a <c>double[,]</c> that contains
    ///     the escape speed required to escape the bound value for each point in input space,
    ///     and secondly contains an <c>int[,]</c> that contains
    ///     the number of iterations required to escape the bound value for each point in input space.
    /// </returns>
    /// <remarks>
    ///     The escape speed in the returns is never more than 1 less than the number of iterations
    ///     for corresponding points.
    /// </remarks>
    public (double[,], int[,]) CalculateEscapeSpeed(bool isTiming = false)
    {
        var startTime = DateTime.Now;
        var result = new double[inputSpace.GetLength(0), inputSpace.GetLength(1)];
        var iterationsResult = new int[inputSpace.GetLength(0), inputSpace.GetLength(1)];
        for (int x = 0; x < inputSpace.GetLength(0); x++)
        {
            Func<int, int> func = i =>
            {
                var resultItem = CalculateEscapeSpeed(inputSpace[x, i]);
                result[x, i] = resultItem.Item1;
                iterationsResult[x, i] = resultItem.Item2;
                return resultItem.Item2;
            };
            Parallel.For(0, inputSpace.GetLength(1), i => func(i));
        }

        var endTime = DateTime.Now;
        if (isTiming)
        {
            Console.WriteLine($"Calculating escape speed took this long: {endTime - startTime}");
        }
        
        return (result, iterationsResult);
    }

    /// <summary>
    ///     For a single point in complex space calculate
    ///     how many iterations are required to escape the bound value.
    /// </summary>
    /// <param name="input">
    ///     The complex point for which the iterations are calculated.
    /// </param>
    /// <returns>
    ///     The number of iterations required to escape the bound value.
    ///     A value of 0 indicates that this point lies inside the fractal.
    /// </returns>
    private int CalculateIteration(Complex input)
    {
        var z = new Complex(0, 0);
        var c = input;
        for (int i = 0; i < _maxIterations; i++)
        {
            if (z.Magnitude > _bound)
            {
                return i;
            }
            else
            {
                z = Func(z, c);
            }
        }
        return 0;
    }
    
    /// <summary>
    ///     For a given input point calculate how quickly it will escape the bound value.
    ///     The value is no more than one less than <see cref="CalculateIteration"/>.
    /// </summary>
    /// <param name="input">
    ///     The complex point for which the escape speed is calculated.
    /// </param>
    /// <returns>
    ///     A tuple firstly containing the escape speed for which the point escapes the bound value,
    ///     and secondly containing the number of iterations required to escape the bound value.
    ///     Higher values indicate slower escape speeds.
    ///     A value of 0 indicates that this point lies inside the fractal.
    /// </returns>
    private (double, int) CalculateEscapeSpeed(Complex input)
    {
        var z = new Complex(0, 0);
        var c = input;
        for (int i = 0; i < _maxIterations; i++)
        {
            if (z.Magnitude > _bound)
            {
                // This is magic and I don't know how it works.
                // Here is an explanation (ostensibly):
                // https://en.wikipedia.org/wiki/Plotting_algorithms_for_the_Mandelbrot_set
                // I do know that nu will always lie in the interval [0, 1)
                var logZn = Math.Log(z.Magnitude);
                var nu = Math.Log(logZn / Math.Log(_bound)) / Math.Log(2);
                var result = i + 1d - nu;
                return (result, i);
            }
            else
            {
                z = Func(z, c);
            }
        }
        return (0d, 0);
    }

    private static Complex[,] GenerateInputSpace(ComplexSpaceParameters inputSpaceParameters)
    {
        var xMin = inputSpaceParameters.XMin;
        var xMax = inputSpaceParameters.XMax;
        var yMin = inputSpaceParameters.YMin;
        var yMax = inputSpaceParameters.YMax;
        var xSize = inputSpaceParameters.XSize;
        var ySize = inputSpaceParameters.YSize;

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

    /// <summary>
    ///     The Mandelbrot function.
    /// </summary>
    /// <param name="z">
    ///     The result of the previous iteration.
    /// </param>
    /// <param name="c">
    ///     The value that corresponds to the point in complex space.
    /// </param>
    /// <returns>
    ///     The result of this iteration.
    /// </returns>
    private static Complex Func(Complex z, Complex c)
    {
        return z * z + c;
    }
}