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
    public class LoyaltyLevelToColorComboBoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] is string level)
            {
                switch (level.ToLower())
                {
                    case "bronze":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 210, 140, 75), 0.0),  // Tamnija brončana
                                new GradientStop(Color.FromArgb(255, 220, 160, 100), 1.0)  // Svjetlija brončana
                            }, 45);

                    case "silver":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 200, 200, 200), 0.0),  // Tamnija srebrna
                                new GradientStop(Color.FromArgb(255, 215, 215, 215), 1.0)  // Svjetlija srebrna
                            }, 45);

                    case "gold":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 255, 220, 50), 0.0),  // Tamnija zlatna
                                new GradientStop(Color.FromArgb(255, 255, 230, 100), 1.0) // Svjetlija zlatna
                            }, 45);

                    case "platinum":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 235, 235, 235), 0.0),  // Tamnija platinasta
                                new GradientStop(Color.FromArgb(255, 245, 245, 245), 1.0)  // Svjetlija platinasta
                            }, 45);

                    case "vip":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 30, 30, 30), 0.0),    // Tamnija crna/siva
                                new GradientStop(Color.FromArgb(255, 50, 50, 50), 1.0)     // Svjetlija crna/siva
                            }, 45);

                    default:
                        // Zadana boja s lakšim prijelazom
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 190, 160, 185), 0.0),  // Tamnija ljubičasta
                                new GradientStop(Color.FromArgb(255, 200, 175, 195), 1.0)  // Svjetlija ljubičasta
                            }, 45);
                }
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
