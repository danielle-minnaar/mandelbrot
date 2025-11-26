using Mandelbrot.src.Model;
using Mandelbrot.src.Model.Parameters;

namespace Mandelbrot.src.Services;

public interface ICalculationService
{
    IterationData Calculate(SpaceParam spaceParam, bool isContinuous);
}