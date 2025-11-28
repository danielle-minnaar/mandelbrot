using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;

namespace Mandelbrot.src.Services;

/// <summary>
///     Interface for the service that performs the calculation.
/// </summary>
public interface ICalculationService
{
    /// <summary>
    ///     Calculate an <see cref="IterationData"/> based on the supplied parameters.
    /// </summary>
    /// <param name="spaceParam">
    ///     The parameter that corresponds to the complex space to be calculated.
    /// </param>
    /// <param name="isContinuous">
    ///     If <c>true</c> calculate escape speed in addition to iterations.
    /// </param>
    /// <returns></returns>
    IterationData Calculate(SpaceParam spaceParam, bool isContinuous);

    /// <summary>
    ///     Gets the progress on the current calculation.
    /// </summary>
    /// <returns>
    ///     A <c>double</c> between 0 and 1.
    /// </returns>
    /// <remarks>
    ///     Progress is a measure of how much of the calculation has been performed
    ///     and not a linear measure of how much longer the calculation will take.
    /// </remarks>
    double GetProgress();
}