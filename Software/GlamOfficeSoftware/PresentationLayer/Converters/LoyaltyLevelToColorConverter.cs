using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PresentationLayer.Converters
{
    public class LoyaltyLevelToColorConverter : IMultiValueConverter
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
                                new GradientStop(Color.FromArgb(255, 205, 127, 50), 0.0),  // Tamnija brončana
                                new GradientStop(Color.FromArgb(255, 218, 165, 105), 1.0)  // Svjetlija brončana
                            }, 45);

                    case "silver":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 192, 192, 192), 0.0),  // Srebrna
                                new GradientStop(Color.FromArgb(255, 224, 224, 224), 1.0)  // Svjetlija srebrna
                            }, 45);

                    case "gold":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 255, 215, 0), 0.0),  // Zlatna
                                new GradientStop(Color.FromArgb(255, 255, 223, 85), 1.0) // Svjetlija zlatna
                            }, 45);

                    case "platinum":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 229, 228, 226), 0.0),  // Tamnija platinasta
                                new GradientStop(Color.FromArgb(255, 250, 250, 250), 1.0)  // Svjetlija platinasta
                            }, 45);

                    case "vip":
                        return new LinearGradientBrush(
                            new GradientStopCollection
                            {
                                new GradientStop(Color.FromArgb(255, 0, 0, 0), 0.0),       // Crna
                                new GradientStop(Color.FromArgb(255, 55, 55, 55), 1.0)    // Tamno siva
                            }, 45);

                    default:
                        // Zadana boja
                        return new SolidColorBrush(Color.FromArgb(255, 184, 148, 172)); // Ljubičasta
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
