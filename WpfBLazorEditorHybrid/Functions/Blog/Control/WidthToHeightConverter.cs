
using System.Globalization;
using System.Windows.Data;

namespace WpfBLazorHybridClient.Functions.Blog.Control
{
    public class WidthToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)value;
            return width * 0.75;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
