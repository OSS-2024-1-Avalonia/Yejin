using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace BlockPuzzle.Converter;

public class BoardImageConverter : IValueConverter
{
    private readonly Bitmap[] _fillTiles = new Bitmap[4];
    public static BoardImageConverter Instance { get; } = new();
    
    private BoardImageConverter()
    {
        _fillTiles[0] = new Bitmap(AssetLoader.Open(new Uri("avares://BlockPuzzle/Assets/Tile.png")));
        for (var i = 1; i < 4; i++)
        {
            _fillTiles[i] = new Bitmap(AssetLoader.Open(new Uri($"avares://BlockPuzzle/Assets/FillTile{i}.png")));
        }
    }
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Console.WriteLine(value);
        return _fillTiles[(int)value];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}