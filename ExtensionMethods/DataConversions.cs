using Mandelbrot.Model;

namespace Mandelbrot.ExtensionMethods;

public static class DataConversions
{
    public static IterationData AddIterationMetaData(this IterationData iterationData)
    {
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

        return iterationData;
    }
}