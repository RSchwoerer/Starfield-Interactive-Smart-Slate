﻿using CommunityToolkit.Mvvm.ComponentModel;
using Starfield_Interactive_Smart_Slate.Models;
using Starfield_Interactive_Smart_Slate.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Starfield_Interactive_Smart_Slate
{
    public class MainViewModel : ObservableObject
    {
        public static MainViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainViewModel();
                }

                return instance;
            }
        }

        public List<Resource> InorganicResources
        {
            get => inorganicResources;
            set => SetProperty(ref inorganicResources, value);
        }

        public List<Resource> OrganicResources
        {
            get => organicResources;
            set => SetProperty(ref organicResources, value);
        }

        public List<Resource> AllResources
        {
            get => allResources;
            set => SetProperty(ref allResources, value);
        }

        public List<SolarSystem> AllSolarSystems
        {
            get => allSolarSystems;
            set => SetProperty(ref allSolarSystems, value);
        }

        public List<SolarSystem> DiscoveredSolarSystems
        {
            get => discoveredSolarSystems;
            set => SetProperty(ref discoveredSolarSystems, value);
        }

        public Dictionary<LifeformType, Dictionary<string, string>> LifeformNames
        {
            get => lifeformNames;
            set => SetProperty(ref lifeformNames, value);
        }

        private static MainViewModel? instance;
        private List<Resource> allResources;
        private List<Resource> inorganicResources;
        private List<Resource> organicResources;
        private List<SolarSystem> allSolarSystems;
        private List<SolarSystem> discoveredSolarSystems;
        private Dictionary<LifeformType, Dictionary<string, string>> lifeformNames;

        private MainViewModel() { }

        public void ReloadAllData()
        {
            // load all resources
            var resources = DataRepository.GetResources();
            AllResources = resources;
            InorganicResources = resources.Where(r => r.GetType() == ResourceType.Inorganic).ToList();
            OrganicResources = resources.Where(r => r.GetType() == ResourceType.Organic).ToList();

            // load all solar systems
            var solarSystems = DataRepository.GetSolarSystems();
            AllSolarSystems = solarSystems;
            DiscoveredSolarSystems = solarSystems
                .Where(solarSystem => solarSystem.Discovered)
                .ToList();

            // load all lifeform names
            LifeformNames = DataRepository.GetLifeformNames();
        }

        public Dictionary<string, string> GetLifeformNames(LifeformType type)
        {
            return lifeformNames[type];
        }

        public void DiscoverSolarSystem(SolarSystem solarSystem)
        {
            solarSystem.Discovered = true;
            DiscoveredSolarSystems = AllSolarSystems
                .Where(solarSystem => solarSystem.Discovered)
                .ToList();
            OnPropertyChanged(nameof(DiscoveredSolarSystems));
        }
    }
}
