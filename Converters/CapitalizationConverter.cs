using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Starfield_Interactive_Smart_Slate.Converters
{
    /// <summary>
    /// Convert text to uppercase.
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/a/66405538
    /// </remarks>
    /// <example>
    /// <TextBlock Text={Binding SomeValue, Converter={is:CapitalizationConverter}}" />
    /// </example>
    [ValueConversion(typeof(string), typeof(string))]
    public class CapitalizationConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value as string)?.ToUpper() ?? value; // If it's a string, call ToUpper(), otherwise, pass it through as-is.

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
