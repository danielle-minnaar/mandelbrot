using Mandelbrot.Model;

namespace Mandelbrot.Helpers;

public interface IBuilderStart
{
    IBuilderWithCalculationResult ExecuteLoopedCalculation(Func<LoopParam, CalcResult[,]> calculationLoop);
}

public interface IBuilderWithCalculationResult
{
    IBuilderWithMetaData AddIterationMetaData();
}

public interface IBuilderWithMetaData
{
    IterationData Build();
}

public class IterationDataBuilder : IBuilderStart, IBuilderWithCalculationResult, IBuilderWithMetaData
{
    private DateTime _calculationStartTime;
    private LoopParam? _loopParam;
    private CalcResult[,]? _calculationResults;
    private int _maxIterations;
    private int _minIterations;
    private int _pointsInFractal;
    private TimeSpan _calculationTime;

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

    public IBuilderWithCalculationResult ExecuteLoopedCalculation(
        Func<LoopParam, CalcResult[,]> calculationLoop)
    {
        CheckIfBuilderHasbeenInitialized(nameof(ExecuteLoopedCalculation));

        _calculationResults = calculationLoop(_loopParam);

        return this;
    }

    public IBuilderWithMetaData AddIterationMetaData()
    {
        CheckIfCalculationHasBeenPerformed(nameof(AddIterationMetaData));

        var queryable = _calculationResults.Cast<CalcResult>();

        _maxIterations = queryable
            .Where(result => result.Iterations != 0)
            .Max(result => result.Iterations);

        _minIterations = queryable
            .Where(result => result.Iterations != 0)
            .Min(result => result.Iterations);

        _pointsInFractal = queryable
            .Where(result => result.Iterations == 0)
            .Count();

        _calculationTime = DateTime.Now - _calculationStartTime;

        return this;
    }

    public IterationData Build()
    {
        CheckIfBuilderHasbeenInitialized(nameof(Build));
        CheckIfCalculationHasBeenPerformed(nameof(Build));
        CheckIfBuilderHasIterationMetaData(nameof(Build));

        return new IterationData
        {
            SpaceParam = _loopParam.SpaceParam,
            Bound = _loopParam.Bound,
            MaxCalculatedIterations = _loopParam.Bound,
            IsContinuous = _loopParam.IsContinuous,
            CalculationResults = _calculationResults,
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

    private void CheckIfBuilderHasbeenInitialized(string currentMethodName)
    {
        if (_loopParam is null)
        {
            throw new MethodAccessException($@"Method {currentMethodName} should not be accessible
                unless {nameof(_loopParam)} has been set by {nameof(Initialize)}");
        }
    }

    private void CheckIfCalculationHasBeenPerformed(string currentMethodName)
    {
        if (_calculationResults is null)
        {
            throw new MethodAccessException($@"Method {currentMethodName} should not be accessible
            unless {nameof(_calculationResults)} has been set by {nameof(ExecuteLoopedCalculation)}");
        }
    }
}