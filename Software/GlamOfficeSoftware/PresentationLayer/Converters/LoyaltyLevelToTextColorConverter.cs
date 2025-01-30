using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PresentationLayer.Converters
{
    public class LoyaltyLevelToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string level)
            {
                switch (level.ToLower())
                {
                    case "gold":
                    case "silver":
                    case "bronze":
                    case "platinum":
                        return new SolidColorBrush(Colors.Black);
                    case "vip":
                        return new SolidColorBrush(Colors.White);
                    default:
                        return new SolidColorBrush(Colors.Black);
                }
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
