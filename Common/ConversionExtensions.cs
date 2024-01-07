using System.Windows.Media;

namespace Starfield_Interactive_Smart_Slate.Common
{
    internal static class ConversionExtensions
    {
        public static SolidColorBrush? ToBrush(this string HexColorString)
        {
            return new BrushConverter().ConvertFrom(HexColorString) as SolidColorBrush;
        }
    }
}
