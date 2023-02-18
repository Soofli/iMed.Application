namespace iMed.App.Converters;
public class TabHeaderSelectedToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool flag)
        {
            if (flag)
                return Application.Current.Resources["PrimaryColor"];
            else
                return Application.Current.Resources["Gray-500"];
        }
        return (Color)Application.Current.Resources["PrimaryColor"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}
