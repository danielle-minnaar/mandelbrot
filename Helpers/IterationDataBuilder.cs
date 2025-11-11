using Mandelbrot.Model;
using Mandelbrot.Model.Parameters;
using Mandelbrot.Generators;
using System.Diagnostics.CodeAnalysis;

namespace Mandelbrot.Helpers;

/// <summary>
///     Interface for after the builder has been initialized.
/// </summary>
public interface IBuilderStart
{
    /// <summary>
    ///     Executes the provided calculation according to
    ///     the <see cref="LoopParam"/> held by the builder.
    /// </summary>
    /// <param name="calculationLoop">
    ///     The calculation should be defined in <see cref="Calculator"/>.
    /// </param>
    /// <returns>
    ///     The next stage of the builder <see cref="IBuilderWithCalculationResult"/>.
    /// </returns>
    IBuilderWithCalculationResult ExecuteLoopedCalculation(
        Func<LoopParam, (double[,]?, int[,])> calculationLoop);
}

/// <summary>
///     Interface for the builder after the calculation has been performed.
/// </summary>
public interface IBuilderWithCalculationResult
{
    /// <summary>
    ///     Based on the result of the calculation determine meta data regarding it.
    /// </summary>
    /// <returns>
    ///     The next stage of the builder <see cref="IBuilderWithMetaData"/>.
    /// </returns>
    IBuilderWithMetaData AddIterationMetaData();
}

/// <summary>
///     Interface for the final stage of the builder.
/// </summary>
public interface IBuilderWithMetaData
{
    /// <summary>
    ///     Using the provided values create an <see cref="IterationData"/> object.
    /// </summary>
    /// <returns>
    ///     The result of the builder.
    /// </returns>
    IterationData Build();
}

/// <summary>
///     A builder to correctly assemble the <see cref="IterationData"/> object.
/// </summary>
public class IterationDataBuilder : IBuilderStart, IBuilderWithCalculationResult, IBuilderWithMetaData
{
    private DateTime _calculationStartTime;
    private LoopParam? _loopParam;
    private int[,]? _iterResult;
    private double[,]? _speedResult;
    private int _maxIterations;
    private int _minIterations;
    private int _pointsInFractal;
    private TimeSpan _calculationTime;

    /// <summary>
    ///     Set the initial values.
    /// </summary>
    /// <param name="loopParam">
    ///     The parameters needed to perform the calculations.
    /// </param>
    /// <returns>
    ///     The first stage of the builder: <see cref="IBuilderStart"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Throws if the bound or max calculated iterations inside of LoopParam are invalid
    /// </exception>
    public IBuilderStart Initialize(LoopParam loopParam)
    {
        if (loopParam.Bound <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(loopParam.Bound),
                "bound cannot be less than or equal to zero");
        }

        if (loopParam.MaxCalculatedIterations <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(loopParam.MaxCalculatedIterations),
                "maxCalculatedIterations cannot be less than or equal to zero");
        }

        _loopParam = loopParam;
        _calculationStartTime = DateTime.Now;

        return this;
    }

    /// <inheritdoc/>
    public IBuilderWithCalculationResult ExecuteLoopedCalculation(
        Func<LoopParam, (double[,]?, int[,])> calculationLoop)
    {
        CheckIfBuilderHasbeenInitialized(nameof(ExecuteLoopedCalculation));

        var result = calculationLoop(_loopParam);
        _iterResult = result.Item2;
        _speedResult = result.Item1;
        return this;
    }

    /// <inheritdoc/>
    public IBuilderWithMetaData AddIterationMetaData()
    {
        CheckIfIterationsAreCalculated(nameof(AddIterationMetaData));
        CheckIfEscapeSpeedShouldHaveBeenCalculated(nameof(AddIterationMetaData));

        var queryable = _iterResult.Cast<int>();

        _maxIterations = queryable
            .Where(result => result != 0)
            .Max();

        _minIterations = queryable
            .Where(result => result != 0)
            .Min();

        _pointsInFractal = queryable
            .Where(result => result == 0)
            .Count();

        _calculationTime = DateTime.Now - _calculationStartTime;

        return this;
    }

    /// <inheritdoc/>
    public IterationData Build()
    {
        CheckIfBuilderHasbeenInitialized(nameof(Build));
        CheckIfIterationsAreCalculated(nameof(Build));
        CheckIfEscapeSpeedShouldHaveBeenCalculated(nameof(Build));
        CheckIfBuilderHasIterationMetaData(nameof(Build));

        return new IterationData
        {
            SpaceParam = _loopParam.SpaceParam,
            Bound = _loopParam.Bound,
            MaxCalculatedIterations = _loopParam.Bound,
            IsContinuous = _loopParam.IsContinuous,
            Iterations = _iterResult,
            EscapeSpeeds = _speedResult,
            MaxIterations = _maxIterations,
            MinIterations = _minIterations,
            PointsInFractal = _pointsInFractal,
            CalculationTime = _calculationTime
        };
    }

    private void CheckIfBuilderHasIterationMetaData(string currentMethodName)
    {
        if (_maxIterations == 0 || _minIterations == 0 || _calculationTime == TimeSpan.Zero)
        {
            throw new MemberAccessException($@"Method {currentMethodName} should not be accessible
                unless {nameof(_maxIterations)}, {nameof(_minIterations)}, {nameof(_pointsInFractal)}
                and {nameof(_calculationTime)} have been set by {nameof(AddIterationMetaData)}.");
        }
    }

    [MemberNotNull(nameof(_loopParam))]
    private void CheckIfBuilderHasbeenInitialized(string currentMethodName)
    {
        if (_loopParam is null)
        {
            throw new MethodAccessException($@"Method {currentMethodName} should not be accessible
                unless {nameof(_loopParam)} has been set by {nameof(Initialize)}");
        }
    }

    [MemberNotNull(nameof(_iterResult))]
    private void CheckIfIterationsAreCalculated(string currentMethodName)
    {
        if (_iterResult is null)
        {
            throw new MethodAccessException($@"Method {currentMethodName} should not be accessible
            unless {nameof(_iterResult)} has been set by {nameof(ExecuteLoopedCalculation)}");
        }
    }

    private void CheckIfEscapeSpeedShouldHaveBeenCalculated(string currentMethodName)
    {
        CheckIfBuilderHasbeenInitialized(currentMethodName);
        if (_loopParam.IsContinuous && _speedResult is null)
        {
            throw new MemberAccessException($@"Method {currentMethodName} should not be accessible
            unless {nameof(_speedResult)} has been set by {nameof(ExecuteLoopedCalculation)}");
        }
    }
}