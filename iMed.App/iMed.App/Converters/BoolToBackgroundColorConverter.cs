namespace iMed.App.Converters;
public class BoolToBackgroundColorConverter : IValueConverter
{
    public Color TrueColor { get; set; }
    public Color FalseColor { get; set; }
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is bool flag)
        {
            if (flag)
                return TrueColor;
            else
                return FalseColor;
        }
        else
        {
            return FalseColor;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}
