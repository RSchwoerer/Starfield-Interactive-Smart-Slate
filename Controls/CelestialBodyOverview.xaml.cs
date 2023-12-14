using Starfield_Interactive_Smart_Slate.Models;
using System.Windows;
using System.Windows.Controls;

namespace Starfield_Interactive_Smart_Slate.Controls
{
    public partial class CelestialBodyOverview : UserControl
    {
        public static readonly DependencyProperty CelestialBodyProperty =
            DependencyProperty.Register(name: "CelestialBody", propertyType: typeof(CelestialBody), ownerType: typeof(CelestialBodyOverview),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: null, flags: FrameworkPropertyMetadataOptions.AffectsRender,
                    propertyChangedCallback: new PropertyChangedCallback(OnCelestialBodyChanged)));

        public CelestialBodyOverview()
        {
            InitializeComponent();
        }

        public CelestialBody CelestialBody
        {
            get => (CelestialBody)GetValue(CelestialBodyProperty);
            set => SetValue(CelestialBodyProperty, value);
        }

        private static void OnCelestialBodyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}