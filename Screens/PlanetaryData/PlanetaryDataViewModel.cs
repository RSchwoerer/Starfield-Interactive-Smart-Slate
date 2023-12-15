using CommunityToolkit.Mvvm.ComponentModel;
using Starfield_Interactive_Smart_Slate.Models;
using Starfield_Interactive_Smart_Slate.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Starfield_Interactive_Smart_Slate.Screens.PlanetaryData
{
    public class PlanetaryDataViewModel : ObservableObject
    {
        public static PlanetaryDataViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlanetaryDataViewModel();
                }

                return instance;
            }
        }

        //public List<SolarSystem> DisplayedSolarSystems
        //{
        //    get => displayedSolarSystems;
        //    set => SetProperty(ref displayedSolarSystems, value);
        //}

        public ICollectionView DisplayedSolarSystems
        {
            get => _DisplayedSolarSystems;
            private set => SetProperty(ref _DisplayedSolarSystems, value);
        }

        public string SolarSystemsFilter
        {
            get => _SolarSystemsFilter;
            set => SetProperty(ref _SolarSystemsFilter, value);
        }

        public bool LifeformFilterEnabled
        {
            get => _LifeformFilterEnabled;
            set
            {
                SetProperty(ref _LifeformFilterEnabled, value);
                DisplayedSolarSystems.Refresh();
            }
        }

        public bool OutpostFilterEnabled
        {
            get => _OutpostFilterEnabled;
            set
            {
                SetProperty(ref _OutpostFilterEnabled, value);
                DisplayedSolarSystems.Refresh();
            }
        }


        // this is used for the lifeform edit combo box, because Terrormorphs have a scanned resource of None
        public List<Resource> SelectableOrganicResources
        {
            get => selectableOrganicResources;
            set => SetProperty(ref selectableOrganicResources, value);
        }

        public CelestialBody? DisplayedCelestialBody
        {
            get => displayedCelestialBody;
            set => SetProperty(ref displayedCelestialBody, value);
        }

        public CelestialBody? SelectedCelestialBody
        {
            get => selectedCelestialBody;
            set => SetProperty(ref selectedCelestialBody, value);
        }

        public Entity? DisplayedEntity
        {
            get => displayedEntity;
            set => SetProperty(ref displayedEntity, value);
        }

        public Entity? SelectedEntity
        {
            get => selectedEntity;
            set => SetProperty(ref selectedEntity, value);
        }

        public int PictureGridColumns
        {
            get => pictureGridColumns;
            set => SetProperty(ref pictureGridColumns, value);
        }

        private static PlanetaryDataViewModel? instance;
        private MainViewModel mainViewModel = MainViewModel.Instance;
        private List<SolarSystem> displayedSolarSystems;
        private List<Resource> selectableOrganicResources;
        private CelestialBody? displayedCelestialBody;
        private CelestialBody? selectedCelestialBody;
        private Entity? selectedEntity;
        private Entity? displayedEntity;
        private ICollectionView _DisplayedSolarSystems;
        private int pictureGridColumns;
        private string _SolarSystemsFilter = string.Empty;
        private bool _LifeformFilterEnabled;
        private bool _OutpostFilterEnabled;

        private PlanetaryDataViewModel()
        {
            LoadSelectableResources();
            mainViewModel.PropertyChanged += HandlePropertyChanged;
        }

        // find solar systems where solar system name or a child celestial body name matches filterText
        // ignores case, whitespace, symbols
        public void SearchSolarSystemsByText(string filterText)
        {
            DisplayedSolarSystems.Refresh();
        }

        public void FilterSolarSystemsByLifeforms()
        {
            // use DiscoveredSolarSystems as base
            DisplayedSolarSystems = CollectionViewSource.GetDefaultView(
                mainViewModel.DiscoveredSolarSystems.Select(
                solarSystem =>
                {
                    solarSystem.ResetShownCelestialBodies();

                    // build ShownCelestialBodies list
                    foreach (var celestialBody in solarSystem.CelestialBodies)
                    {
                        var surfaceHasLifeform = celestialBody.HasLifeform;
                        var moonsHaveOutposts = celestialBody.Moons?.Any(moon => moon.HasLifeform) ?? false;

                        // if celestial body (or any of its moons) has lifeform, include it in the list
                        if (surfaceHasLifeform || moonsHaveOutposts)
                        {
                            celestialBody.GrayOut = !surfaceHasLifeform;
                            solarSystem.ShownCelestialBodies.Add(celestialBody);
                        }
                    }

                    return solarSystem;
                }
            ).Where(solarSystem => solarSystem.ShownCelestialBodies.Any())
            .ToList());
        }

        public void FilterSolarSystemsByOutposts()
        {
            // use DiscoveredSolarSystems as base
            DisplayedSolarSystems = CollectionViewSource.GetDefaultView(
                mainViewModel.DiscoveredSolarSystems.Select(
                solarSystem =>
                {
                    solarSystem.ResetShownCelestialBodies();

                    // build ShownCelestialBodies list
                    foreach (var celestialBody in solarSystem.CelestialBodies)
                    {
                        var surfaceHasOutpost = celestialBody.HasOutpost;
                        var moonsHaveOutposts = celestialBody.Moons?.Any(moon => moon.HasOutpost) ?? false;

                        // if celestial body (or any of its moons) has an outpost, include it in the list
                        if (surfaceHasOutpost || moonsHaveOutposts)
                        {
                            celestialBody.GrayOut = !surfaceHasOutpost;
                            solarSystem.ShownCelestialBodies.Add(celestialBody);
                        }
                    }

                    return solarSystem;
                }
            ).Where(solarSystem => solarSystem.ShownCelestialBodies.Any())
            .ToList());
        }

        public void ResetAllFilters()
        {
            DisplayedSolarSystems.Refresh();
            DisplayedSolarSystems = InitializeSolarSystemCollection();

            foreach (var solarSystem in mainViewModel.DiscoveredSolarSystems)
            {
                solarSystem.ShowAllCelestialBodies();
            }
        }

        private void LoadSelectableResources()
        {
            if (mainViewModel.AllResources != null)
            {
                SelectableOrganicResources = mainViewModel.AllResources.Where(r =>
                {
                    return r.GetType() == ResourceType.Organic || r.GetType() == ResourceType.Placeholders;
                })
                .OrderBy(r => r.FullName)
                .ToList();

                // add placeholder to allow un-setting lifeform resource
                SelectableOrganicResources.Insert(
                    0, new Resource(-1, ResourceType.Placeholders, "Unknown", null, Rarity.Common));
            }
        }

        private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(mainViewModel.AllResources):
                    LoadSelectableResources();
                    break;
                case nameof(mainViewModel.DiscoveredSolarSystems):
                    DisplayedSolarSystems = InitializeSolarSystemCollection();
                    break;
            }
        }

        private ICollectionView InitializeSolarSystemCollection()
        {
            var collectionView = CollectionViewSource.GetDefaultView(mainViewModel.DiscoveredSolarSystems);
            collectionView.Filter = item =>
            {
                if (item is SolarSystem solarSystem)
                {
                    //if (LifeformFilterEnabled || OutpostFilterEnabled)
                    //{
                    //    var found = false;
                    //    solarSystem.CelestialBodies.ForEach(c =>
                    //    {
                    //        var surfaceHasOutpost = (OutpostFilterEnabled && c.HasOutpost) ||
                    //                                (LifeformFilterEnabled && c.HasLifeform);
                    //        var moonsHaveOutposts = (OutpostFilterEnabled && (c.Moons?.Any(moon => moon.HasOutpost) ?? false)) ||
                    //                                (LifeformFilterEnabled && (c.Moons?.Any(moon => moon.HasLifeform) ?? false));
                    //        c.GrayOut = !surfaceHasOutpost;

                    //        // if celestial body (or any of its moons) has an outpost, include it in the list
                    //        if (surfaceHasOutpost || moonsHaveOutposts)
                    //        {
                    //            //c.GrayOut = !surfaceHasOutpost;
                    //            //solarSystem.ShownCelestialBodies.Add(celestialBody);
                    //            //return true;
                    //            found = true;
                    //        }
                    //        //return false;
                    //    });
                    //    return found;
                    //}

                    var sn = solarSystem.SystemName.ToLower();
                    var flt = SolarSystemsFilter.ToLower();

                    return
                        SolarSystemsFilter.Length > 1
                            ? sn.Contains(flt) ||
                              solarSystem.CelestialBodies.Any(b => b.BodyName.ToLower().Contains(flt))
                            : solarSystem.SystemName.ToLower().StartsWith(flt) ||
                              solarSystem.CelestialBodies.Any(b => b.BodyName.ToLower().StartsWith(flt));
                }
                return false;
            };
            return collectionView;
        }
    }
}
