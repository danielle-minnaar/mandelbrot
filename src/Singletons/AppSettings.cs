namespace Mandelbrot.src.Singletons;

/// <summary>
///     Holds application-level configuration settings.
///     These values are loaded from appsettings.json
/// </summary>
public class AppSettings
{
    /// <summary>
    ///     An array that holds the location of the directory that holds all the png color palette files.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="GetColorsLocation"/> to get an execution context sensitive string.
    /// </remarks>
    public required string[] ColorsLocation { get; set; }
    
    /// <summary>
    ///     An array that holds the relative path of the json file that is the color palette db.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="GetColorPaletteDbLocation"/> to get an execution context sensitive string.
    /// </remarks>
    public required string[] ColorPaletteDbLocation { get; set; }

    /// <summary>
    ///     The number of iterations used for the first calculation.
    /// </summary>
    public required int InitialIterations { get; set; }
    
    /// <summary>
    ///     The factor by which the iterations are increased
    ///     relative to the minimum number of iterations of the previous calculation.
    /// </summary>
    public required int IterationFactor { get; set; }

    /// <summary>
    ///     Get the execution context sensitive location.
    /// </summary>
    public string GetColorsLocation()
    {
        return Path.Join(AppContext.BaseDirectory, Path.Join(ColorsLocation));
    }

    /// <summary>
    ///     Get the execution context sensitive location.
    /// </summary>
    public string GetColorPaletteDbLocation()
    {
        return Path.Join(AppContext.BaseDirectory, Path.Join(ColorPaletteDbLocation));
    }
}