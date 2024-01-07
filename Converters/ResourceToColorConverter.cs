using Starfield_Interactive_Smart_Slate.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Starfield_Interactive_Smart_Slate.Converters
{
    internal class ResourceToColorConverter : IValueConverter
    {
        /// <summary>
        /// source: https://www.reddit.com/r/Starfield/comments/16fcivg/inorganic_resource_relation_chart/
        /// </summary>
        static readonly Dictionary<string, SolidColorBrush?> ResourceColors =
            new Dictionary<string, SolidColorBrush?>
            {
                {"", Brushes.Transparent},
                {"He-3",  "#729e59".ToBrush()},
                {"H2O",  "#68918c".ToBrush()},
                {"Ar",  "#164160".ToBrush()},
                {"C6Hn",  "#1d5579".ToBrush()},
                {"R-COC",  "#276d90".ToBrush()},
                {"Ne",  "#0192b1".ToBrush()},
                {"Vr",  "#00c9dc".ToBrush()},
                {"Fe",  "#6e342f".ToBrush()},
                {"HnCn",  "#9b3c45".ToBrush()},
                {"Ta",  "#b94b4e".ToBrush()},
                {"Yb",  "#d26d54".ToBrush()},
                {"Rc",  "#ff7759".ToBrush()},
                {"Al",  "#29403f".ToBrush()},
                {"Be",  "#2b5c59".ToBrush()},
                {"Nd",  "#568b86".ToBrush()},
                {"Eu",  "#80c0ba".ToBrush()},
                {"Ie",  "#bbfdfd".ToBrush()},
                {"U",  "#3f4007".ToBrush()},
                {"Ir",  "#636119".ToBrush()},
                {"V",  "#7e7e21".ToBrush()},
                {"Pu",  "#a6a624".ToBrush()},
                {"Vy",  "#e0e01c".ToBrush()},
                {"Cl",  "#37404f".ToBrush()},
                {"SiH3Cl",  "#41518b".ToBrush()},
                {"Li",  "#4865c8".ToBrush()},
                {"Xe",  "#6c86ff".ToBrush()},
                {"Ad",  "#b176fe".ToBrush()},
                {"Cs",  "#7a70ff".ToBrush()},
                {"Pb",  "#39464a".ToBrush()},
                {"W",  "#4b5e68".ToBrush()},
                {"Ti",  "#587886".ToBrush()},
                {"Dy",  "#5d92a9".ToBrush()},
                {"Ag",  "#70655e".ToBrush()},
                {"Hg",  "#9b8276".ToBrush()},
                {"Cu",  "#1a4c3f".ToBrush()},
                {"F",  "#206b5b".ToBrush()},
                {"xF4",  "#1d9079".ToBrush()},
                {"IL",  "#00c9a8".ToBrush()},
                {"Au",  "#bc8a34".ToBrush()},
                {"Sb",  "#d0ac3d".ToBrush()},
                {"Ni",  "#573913".ToBrush()},
                {"Co",  "#274d87".ToBrush()},
                {"Pt",  "#3865a1".ToBrush()},
                {"Pd",  "#d38d1a".ToBrush()},
                {"Tsn",  "#f9c906".ToBrush()},
                {"Ct",  "#ff6000".ToBrush()},
            };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var shortName = value as string ?? "";
            return ResourceColors[shortName] ?? Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
