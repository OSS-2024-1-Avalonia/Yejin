using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace BlockPuzzle.Converter;

public class BlockImageConverter : IValueConverter
{
    private readonly Bitmap _colorImage;
    private readonly Bitmap _grayImage;
    public static BlockImageConverter Instance { get; } = new();
    
    private BlockImageConverter()
    {
        _colorImage = new Bitmap(AssetLoader.Open(new Uri("avares://BlockPuzzle/Assets/Block.png")));
        _grayImage = new Bitmap(AssetLoader.Open(new Uri("avares://BlockPuzzle/Assets/Gray.png")));
    }
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)value ? _colorImage : _grayImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}