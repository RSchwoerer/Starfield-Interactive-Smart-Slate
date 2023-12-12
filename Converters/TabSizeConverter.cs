using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Starfield_Interactive_Smart_Slate.Converters
{
    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var tabControl = values[0] as TabControl;
            double width = tabControl.ActualWidth / tabControl.Items.Count;

            var tabPanel = values[1] as Panel;
            double width2 = tabPanel.ActualWidth / tabControl.Items.Count;

            // Subtract 1, otherwise we could overflow to two rows.
            return width2 <= 1 ? 0 : width2 - 1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
