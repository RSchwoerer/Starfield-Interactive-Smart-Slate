﻿using Starfield_Interactive_Smart_Slate.Models.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Starfield_Interactive_Smart_Slate.Models
{
    public class CelestialBody : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int BodyID { get; set; }
        public string BodyName { get; set; }
        public string SystemName { get; set; }
        public bool IsMoon { get; set; }
        public string BodyType { get; set; }
        public double Gravity { get; set; }
        public string Temperature { get; set; }
        public string Atmosphere { get; set; }
        public string Magnetosphere { get; set; }
        public string Water { get; set; }
        public int TotalFauna { get; set; }
        public int TotalFlora { get; set; }
        public List<Resource>? Resources { get; set; }
        public ObservableCollection<Fauna>? Faunas { get; set; }
        public ObservableCollection<Flora>? Floras { get; set; }
        public ObservableCollection<Outpost>? Outposts { get; set; }

        // helper attributes to display resource search
        public List<CelestialBody>? Moons;
        public bool Show { get; set; }
        public bool GrayOut { get; set; }

        public string? LifeformProgress
        {
            get
            {
                if (TotalFauna > 0 || TotalFlora > 0)
                {
                    int surveyedFaunas = Faunas?.Where(fauna => fauna.IsSurveyed).Count() ?? 0;
                    int faunasPoints = surveyedFaunas + (Faunas?.Count ?? 0);
                    int surveyedFloras = Floras?.Where(flora => flora.IsSurveyed).Count() ?? 0;
                    int florasPoints = surveyedFloras + (Floras?.Count ?? 0);
                    double surveyPercent = 100.0 * (faunasPoints + florasPoints) / ((TotalFauna + TotalFlora) * 2);
                    return $"🧬 ({surveyPercent:F0}%)";
                }
                else
                {
                    return null;
                }
            }
        }

        public string ResourcesString
        {
            get
            {
                if (Resources != null)
                {
                    return $"{string.Join("\n", Resources.Select(r =>
                    {
                        return r.PrettifiedName;
                    }))}";
                }
                else
                {
                    return "None";
                }
            }
        }

        public bool HasOutpost
        {
            get
            {
                return Outposts != null && Outposts.Count > 0;
            }
        }

        public bool HasLifeform
        {
            get
            {
                return TotalFauna > 0 || TotalFlora > 0;
            }
        }

        public override string ToString()
        {
            string overviewString = $"Type: {BodyType}\n" +
                $"Gravity: {Gravity}\n" +
                $"Temperature: {Temperature}\n" +
                $"Atmosphere: {Atmosphere}\n" +
                $"Magnetosphere: {Magnetosphere}\n" +
                $"Fauna: {GetFaunaCountString()}\n" +
                $"Flora: {GetFloraCountString()}\n" +
                $"Water: {Water}";

            return overviewString;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CelestialBody)
            {
                return BodyID == ((CelestialBody)obj).BodyID;
            }
            else
            {
                return false;
            }
        }

        public CelestialBody DeepCopy()
        {
            // TODO: rework view model management
            //ObservableCollection<Fauna> faunaCollection = null;
            //if (Faunas != null)
            //{
            //    faunaCollection = new ObservableCollection<Fauna>(
            //        Faunas.Select(fauna => fauna.DeepCopy(fast))
            //    );
            //}

            //ObservableCollection<Flora> floraCollection = null;
            //if (Floras != null)
            //{
            //    floraCollection = new ObservableCollection<Flora>(
            //        Floras.Select(flora => flora.DeepCopy(fast))
            //    );
            //}

            //ObservableCollection<Outpost> outpostCollection = null;
            //if (Outposts != null)
            //{
            //    outpostCollection = new ObservableCollection<Outpost>(
            //        Outposts.Select(outpost => outpost.DeepCopy(fast))
            //    );
            //}

            return new CelestialBody
            {
                BodyID = BodyID,
                BodyName = BodyName,
                SystemName = SystemName,
                IsMoon = IsMoon,
                BodyType = BodyType,
                Gravity = Gravity,
                Temperature = Temperature,
                Atmosphere = Atmosphere,
                Magnetosphere = Magnetosphere,
                Water = Water,
                TotalFauna = TotalFauna,
                TotalFlora = TotalFlora,
                Resources = Resources,
                //Resources = Resources?.ConvertAll(resource => resource.DeepCopy()),
                Faunas = Faunas,
                Floras = Floras,
                Outposts = Outposts,
                // Moons will be handled by the SolarSystem for now
                //Moons = Moons?.ConvertAll(moon => moon.DeepCopy()),
                Show = Show,
                GrayOut = GrayOut
            };
        }

        public void NotifyLayoutUpdate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LifeformProgress)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasOutpost)));
        }

        public void AddFauna(Fauna fauna)
        {
            if (Faunas == null)
            {
                Faunas = new ObservableCollection<Fauna>();
            }
            Faunas.Add(fauna);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LifeformProgress)));
        }

        public void AddFlora(Flora flora)
        {
            if (Floras == null)
            {
                Floras = new ObservableCollection<Flora>();
            }
            Floras.Add(flora);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LifeformProgress)));
        }

        public void AddOutpost(Outpost outpost)
        {
            if (Outposts == null)
            {
                Outposts = new ObservableCollection<Outpost>();
            }
            Outposts.Add(outpost);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasOutpost)));
        }

        public void DeleteOutpost(Outpost deletedOutpost)
        {
            foreach (var outpost in Outposts)
            {
                if (outpost.ID == deletedOutpost.ID)
                {
                    Outposts.Remove(outpost);
                    break;
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasOutpost)));
        }

        public void EditFauna(Fauna editedFauna)
        {
            foreach (var fauna in Faunas)
            {
                if (fauna.ID == editedFauna.ID)
                {
                    Faunas[Faunas.IndexOf(fauna)] = editedFauna;
                    break;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LifeformProgress)));
        }

        public void EditFlora(Flora editedFlora)
        {
            foreach (var flora in Floras)
            {
                if (flora.ID == editedFlora.ID)
                {
                    Floras[Floras.IndexOf(flora)] = editedFlora;
                    break;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LifeformProgress)));
        }

        public void EditOutpost(Outpost editedOutpost)
        {
            foreach (var outpost in Outposts)
            {
                if (outpost.ID == editedOutpost.ID)
                {
                    Outposts[Outposts.IndexOf(outpost)] = editedOutpost;
                    break;
                }
            }
        }

        public void AddMoon(CelestialBody celestialBody)
        {
            if (Moons == null)
            {
                Moons = new List<CelestialBody>();
            }

            Moons.Add(celestialBody);
        }

        public bool SurfaceContainsResource(Resource resource)
        {
            return Resources?.Contains(resource) ?? false;
        }

        public (bool, ObservableCollection<Fauna>?, ObservableCollection<Flora>?) GetLifeformsWithResource(Resource resource)
        {
            if (TotalFauna == 0 && TotalFlora == 0)
            {
                return (false, null, null); // short circuit for celestial bodies with no life
            }

            var found = false;

            var faunaList = Faunas?.Where(
                fauna =>
                {
                    if (fauna.PrimaryDrops?.Any(drop => drop.Equals(resource)) ?? false)
                    {
                        found = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            ).ToList();

            ObservableCollection<Fauna> faunaCollection = null;
            if (faunaList != null)
            {
                faunaCollection = new ObservableCollection<Fauna>(faunaList);
            }

            var floraList = Floras?.Where(
                flora =>
                {
                    if (flora.PrimaryDrops?.Any(drop => drop.Equals(resource)) ?? false)
                    {
                        found = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            ).ToList();

            ObservableCollection<Flora> floraCollection = null;
            if (floraList != null)
            {
                floraCollection = new ObservableCollection<Flora>(floraList);
            }

            return (found, faunaCollection, floraCollection);
        }

        private string GetFaunaCountString()
        {
            if ((Faunas?.Count ?? 0 + TotalFauna) == 0)
            {
                return "None";
            }
            else
            {
                return $"{Faunas?.Count ?? 0}/{TotalFauna}";
            }
        }

        private string GetFloraCountString()
        {
            if ((Floras?.Count ?? 0 + TotalFlora) == 0)
            {
                return "None";
            }
            else
            {
                return $"{Floras?.Count ?? 0}/{TotalFlora}";
            }
        }
    }
}
