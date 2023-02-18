namespace iMed.App.Converters;
public class DoubleToRoundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double x)
            return Math.Round(x, 1);
        else
            return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return 0;
    }
}