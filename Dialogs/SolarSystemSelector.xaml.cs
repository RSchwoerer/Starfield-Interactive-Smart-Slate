using Starfield_Interactive_Smart_Slate.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Starfield_Interactive_Smart_Slate
{
    public partial class SolarSystemSelector : Window, INotifyPropertyChanged
    {
        private SolarSystem? selectedSolarSystem;

        public SolarSystemSelector(List<SolarSystem> solarSystems)
        {
            AllSolarSystems = solarSystems;

            InitializeComponent();
            SolarSystemFilterTextBox.Focus();

            this.PreviewKeyDown += OnPreviewKeyDown;

            AllSolarSystemsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(AllSolarSystems);
            AllSolarSystemsCollectionView.Filter += OnAllSolarSystemsCollectionViewFilter;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<SolarSystem> AllSolarSystems { get; set; }

        public CollectionView AllSolarSystemsCollectionView { get; private set; }

        public bool HasItemSelected => SelectedSolarSystem != null;

        public SolarSystem? SelectedSolarSystem
        {
            get => selectedSolarSystem;
            set
            {
                selectedSolarSystem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSolarSystem)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasItemSelected)));
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void DiscoverButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private bool OnAllSolarSystemsCollectionViewFilter(object obj)
        {
            var s = (SolarSystem)obj;
            return s.SystemName
                .ToLower()
                .Contains(SolarSystemFilterTextBox.Text.ToLower());
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab && SolarSystemFilterTextBox.IsFocused)
            {
                // try
                if (AllSolarSystemsListBox.Items.Count > 0)
                {
                    var listBoxItem = (ListBoxItem)AllSolarSystemsListBox
                     .ItemContainerGenerator
                     .ContainerFromItem(AllSolarSystemsListBox.Items[0]);
                    listBoxItem.Focus();
                    AllSolarSystemsListBox.SelectedItem = AllSolarSystemsListBox.Items[0];
                }

                e.Handled = true;
            }

            if (char.TryParse(e.Key.ToString(), out char c) &&
                char.IsLetter(c) &&
                !SolarSystemFilterTextBox.IsFocused)
            {
                SolarSystemFilterTextBox.Clear();
                SolarSystemFilterTextBox.Focus();
                SelectedSolarSystem = null;
            }

            if (e.Key == Key.Enter && SelectedSolarSystem != null)
            {
                DialogResult = true;
                Close();
                e.Handled = true;
            }

            if (e.Key == Key.Escape && SelectedSolarSystem != null)
            {
                SolarSystemFilterTextBox.Clear();
                SolarSystemFilterTextBox.Focus();
                SelectedSolarSystem = null;
                e.Handled = true;
            }
            else if (e.Key == Key.Escape && SelectedSolarSystem == null)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void SolarSystemFilterChanged(object sender, TextChangedEventArgs e)
        {
            AllSolarSystemsCollectionView.Refresh();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DialogResult == true)
            {
                App.Current.PlayClickSound();
            }
            else
            {
                App.Current.PlayCancelSound();
            }
        }
    }
}