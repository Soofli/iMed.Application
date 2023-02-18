namespace iMed.App.Converters;

public class DateTimeToRemainingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
            {
                var diff = DateTimeExtensions.Difference(DateTime.Now.Date, date.Date);
                return $"{diff.Days} روز گذشته";
            }
            else
            {
                var diff = DateTimeExtensions.Difference(DateTime.Now.Date, date.Date);
                if (diff == TimeSpan.Zero)
                {
                    return "امروز";
                }
                return $"{diff.Days} روز باقی مانده";
            }
        }
        else
            return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}