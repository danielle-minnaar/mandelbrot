namespace Mandelbrot.src.Singletons;

/// <summary>
///     Holds application-level configuration settings.
///     These values are loaded from appsettings.json
/// </summary>
public class AppSettings
{
    /// <summary>
    ///     The name of the directory that holds all the png color palette files.
    /// </summary>
    public required string ColorsLocation { get; set; }
    
    /// <summary>
    ///     The name of the json file that is the color palette db.
    /// </summary>
    public required string ColorPaletteDbLocation { get; set; }
}