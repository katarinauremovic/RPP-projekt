using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PresentationLayer.Converters
{
    public class LoyaltyLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string level = value as string;
            if (level == null)
                return Brushes.Transparent;

            switch (level.ToLower())
            {
                case "gold":
                    return Brushes.Gold;
                case "silver":
                    return Brushes.Silver;
                case "bronze":
                    return Brushes.SandyBrown;
                case "platinum":
                    return Brushes.Plum;
                case "vip":
                    return Brushes.MediumPurple;
                default:
                    return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
