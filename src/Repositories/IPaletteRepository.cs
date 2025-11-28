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
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
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
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    Task<ColorPalette> GetByName(string name);
    
    /// <summary>
    ///     Get all color palettes in storage.
    /// </summary>
    /// <returns>
    ///     An array of color palettes.
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    Task<ColorPalette[]> GetAll();
    
    /// <summary>
    ///     Get a color palette by id.
    /// </summary>
    /// <returns>
    ///     A color palette.
    /// </returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    Task<ColorPalette> GetById(Guid id);
    
    /// <summary>
    ///     Modify a color palette.
    /// </summary>
    /// <param name="item">
    ///     The color palette with updated fields.
    /// </param>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    Task Update(ColorPalette item);

    /// <summary>
    ///     Create a new color palette.
    /// </summary>
    /// <param name="item">
    ///     The new color palette
    /// </param>
    /// <exception cref="FileNotFoundException"></exception>
    Task CreateItem(ColorPalette item);
}