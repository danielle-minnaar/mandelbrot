using Mandelbrot.src.Model;

namespace Mandelbrot.src.Repositories;

/// <summary>
///     Interface for color palette repository.
/// </summary>
public interface IPaletteRepository
{
    /// <summary>
    ///     Get the most recently used color palette.
    /// </summary>
    /// <returns>
    ///     A color palette.
    /// </returns>
    Task<ColorPalette> GetMostRecent();
    
    /// <summary>
    ///     Get a color palette by name.
    /// </summary>
    /// <param name="name">
    ///     The name of the item to be found.
    /// </param>
    /// <returns>
    ///     A color palette
    /// </returns>
    Task<ColorPalette> GetByName(string name);
    
    /// <summary>
    ///     Get all color palettes in storage.
    /// </summary>
    /// <returns>
    ///     An array of color palettes.
    /// </returns>
    Task<ColorPalette[]> GetAll();
    
    /// <summary>
    ///     Get a color palette by id.
    /// </summary>
    /// <returns>
    ///     A color palette.
    /// </returns>
    Task<ColorPalette> GetById(Guid id);
    
    /// <summary>
    ///     Modify a color palette.
    /// </summary>
    /// <param name="item">
    ///     The color palette with updated fields.
    /// </param>
    Task Update(ColorPalette item);

    /// <summary>
    ///     Create a new color palette.
    /// </summary>
    /// <param name="item">
    ///     The new color palette
    /// </param>
    Task CreateItem(ColorPalette item);
}