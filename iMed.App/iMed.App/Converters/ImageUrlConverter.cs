namespace iMed.App.Converters;

public class ImageUrlConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string url)
            return $"{Addresses.DownloadImage}/{value}";
        else
            return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}