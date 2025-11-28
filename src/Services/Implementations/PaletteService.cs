using System.Diagnostics.CodeAnalysis;
using Mandelbrot.src.Dtos;
using Mandelbrot.src.ExtensionMethods;
using Mandelbrot.src.Helpers.ColorKernels;
using Mandelbrot.src.Model;
using Mandelbrot.src.Repositories;

namespace Mandelbrot.src.Services.Implementations;

/// <inheritdoc/>
public class PaletteService : IPaletteService
{
    /// <inheritdoc/>
    public ColorPalette CurrentPalette {
        get
        {
            if (_currentPalette is null)
            {
                InitializeAsync()
                    .GetAwaiter()
                    .GetResult();
            }
            return _currentPalette;
        } private set
        {
            _currentPalette = value;
        } }

    /// <inheritdoc/>
    public ColorType ColorType { get; private set; }

    private readonly IPaletteRepository _palettes;
    private ColorPalette? _currentPalette;

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="PaletteService"/> class.
    /// </summary>
    /// <param name="palettes">
    ///     The palette repository, handled by DI.
    /// </param>
    public PaletteService(IPaletteRepository palettes)
    {
        _palettes = palettes;
        ColorType = ColorType.Dithered;
    }

    /// <inheritdoc/>
    [MemberNotNull(nameof(_currentPalette))]
    public async Task InitializeAsync()
    {
        _currentPalette = _palettes
            .GetMostRecent()
            .GetAwaiter()
            .GetResult();
    }

    /// <inheritdoc/>
    public void ChangeColorType(ColorType newType)
    {
        ColorType = newType;
    }

    /// <inheritdoc/>
    public async Task ChangePalette(string paletteName)
    {
        CurrentPalette = await _palettes.GetByName(paletteName);
    }

    /// <inheritdoc/>
    public void UpdatePalette(ColorPalettePatchDto newValues)
    {
        if (!newValues.DitherRatio.HasValue) 
        {
            newValues.DitherRatio = CurrentPalette.DitherRatio;
        }
        if (!newValues.ColorSkew.HasValue)
        {
            newValues.ColorSkew = CurrentPalette.ColorSkew;
        }

        // Conversion ensures that values are in acceptable range.
        var temp = new ColorPalettePostDto
        {
            ColorSkew = newValues.ColorSkew.Value,
            DitherRatio = newValues.DitherRatio.Value,
            Name = CurrentPalette.Name
        }.ToModel();

        CurrentPalette.DitherRatio = temp.DitherRatio;
        CurrentPalette.ColorSkew = temp.ColorSkew;
    }

    /// <inheritdoc/>
    public async Task SavePalette()
    {
        await _palettes.Update(CurrentPalette);
    }
}