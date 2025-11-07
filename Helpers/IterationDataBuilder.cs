using Mandelbrot.Model;

namespace Mandelbrot.Helpers;

public interface IBuilderStart
{
    IBuilderWithCalculationResult AddCalculationResult(CalculationResult calculationResult);
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
    private IterationData? iterationData;
    private DateTime calculationStartTime;

    public IBuilderStart Initialize(ComplexSpaceParameters spaceParam, int maxIterations, int bound)
    {
        calculationStartTime = DateTime.Now;
        iterationData = new IterationData
        {
            ComplexSpaceParameters = spaceParam,
            MaxCalculatedIterations = maxIterations,
            Bound = bound
        };

        return this;
    }

    public IBuilderWithCalculationResult AddCalculationResult(
        CalculationResult calculationResult)
    {
        iterationData.Iterations = calculationResult.Iterations;
        iterationData.EscapeSpeeds = calculationResult.EscapeSpeeds ?? null;
        iterationData.CalculationTime = DateTime.Now - calculationStartTime;

        return this;
    }

    public IBuilderWithMetaData AddIterationMetaData()
    {
        if (iterationData.Iterations is null)
        {
            throw new MethodAccessException(@"The field Iterations in the class IterationData was empty.
                This method should not be accessible before Iterations is filled");
        }
        var queryable = iterationData.Iterations.Cast<int>();

        iterationData.MaxIterations = queryable
            .Where(i => i != 0)
            .Max();

        iterationData.MinIterations = queryable
            .Where(i => i != 0)
            .Min();

        iterationData.PointsInFractal = queryable
            .Where(i => i == 0)
            .Count();

        return this;
    }
    
    public IterationData Build()
    {
        return iterationData;
    }
}