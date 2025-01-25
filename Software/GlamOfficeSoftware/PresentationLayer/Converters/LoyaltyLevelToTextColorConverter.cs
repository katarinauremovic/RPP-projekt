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
                        return new SolidColorBrush(Colors.Black); // Tamni tekst za svijetle pozadine
                    case "vip":
                        return new SolidColorBrush(Colors.White); // Svijetli tekst za tamne pozadine
                    default:
                        return new SolidColorBrush(Colors.Black); // Zadano
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
