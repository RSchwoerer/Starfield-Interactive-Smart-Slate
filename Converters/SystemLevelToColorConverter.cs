using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Starfield_Interactive_Smart_Slate.Converters
{
    public class SystemLevelToColorConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int level)
            {
                switch (level)
                {
                    case > 0 and <= 14:
                        // green
                        return new BrushConverter().ConvertFrom("#00cc00") as SolidColorBrush;

                    case >= 15 and <= 24:
                        // blue
                        return new BrushConverter().ConvertFrom("#68a0b9") as SolidColorBrush;

                    case >= 25 and <= 34:
                        // yellow
                        return new BrushConverter().ConvertFrom("#ceb125") as SolidColorBrush;

                    case >= 35 and <= 49:
                        // orange
                        return new BrushConverter().ConvertFrom("#e87910") as SolidColorBrush;

                    case >= 50:
                        // red
                        return new BrushConverter().ConvertFrom("#e61c24") as SolidColorBrush;
                }
            }

            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}