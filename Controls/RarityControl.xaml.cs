using Starfield_Interactive_Smart_Slate.Models;
using System.Windows;
using System.Windows.Controls;

namespace Starfield_Interactive_Smart_Slate.Controls
{
    public partial class RarityControl : UserControl
    {
        public static readonly DependencyProperty RarityProperty =
           DependencyProperty.Register(name: "Rarity", propertyType: typeof(Rarity), ownerType: typeof(RarityControl),
               typeMetadata: new FrameworkPropertyMetadata(defaultValue: Rarity.Common, flags: FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                   propertyChangedCallback: new PropertyChangedCallback(OnRarityChanged)));

        public RarityControl()
        {
            InitializeComponent();
        }

        public Rarity Rarity
        {
            get => (Rarity)GetValue(RarityProperty);
            set => SetValue(RarityProperty, value);
        }

        private static void OnRarityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}