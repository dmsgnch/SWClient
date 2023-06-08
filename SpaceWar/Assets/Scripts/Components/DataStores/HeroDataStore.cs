using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using Assets.Scripts.View;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Components.DataStores
{
    public class HeroDataStore : IDataStore
    {
        private int resourses;
        private int soldiersLimit;
        private int availableSoldiers;
        private byte researchShipLimit;
        private byte availableResearchShips = 0;
        private byte colonizationShipLimit;
        private byte availableColonizationShips = 0;


        /// <summary>
        /// The id of the session that we receive after start button press and use to create a requests
        /// </summary>
        public Guid HeroId { get; set; }
        public string Name { get; set; }

        public int Resourses
        {
            get => resourses;
            set
            {
                var diff = value - resourses;
                resourses = value;
                if (diff == 0) return;
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callResourcesChangedPanel($"Resources\n({symbol}{diff})");
            }
        }
        public int SoldiersLimit
        {
            get => soldiersLimit;
            set
            {
                var diff = value - soldiersLimit;
                soldiersLimit = value;
                if (diff == 0) return;
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callSoldiersChangedPanel($"Soldiers limit\n({symbol}{diff})");
            }
        }
        public int AvailableSoldiers
        {
            get => availableSoldiers;
            set
            {
                var diff = value - availableSoldiers;
                availableSoldiers = value;
                if (diff == 0) return;
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callSoldiersChangedPanel($"Available soldiers\n({symbol}{diff})");
            }
        }
        public byte ResearchShipLimit
        {
            get => researchShipLimit;
            set
            {
                var diff = value - researchShipLimit;
                researchShipLimit = value;
                if (diff == 0) return;
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callResearchShipsChangedPanel($"Research ship limit\n({symbol}{diff})");
            }
        }
        public byte AvailableResearchShips
        {
            get => availableResearchShips;
            set
            {
                var diff = value - availableResearchShips;
                availableResearchShips = value;
                if (diff == 0) return;
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callResearchShipsChangedPanel($"Available research ships\n({symbol}{diff})");
            }
        }
        public byte ColonizationShipLimit
        {
            get => colonizationShipLimit;
            set
            {
                var diff = value - colonizationShipLimit;
                colonizationShipLimit = value;
                if (diff == 0) return;
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callColonizeShipsChangedPanel($"Colonization ship limit\n({symbol}{diff})");
            }
        }
        public byte AvailableColonizationShips {
            get => availableColonizationShips;
            set {
                var diff = value - availableColonizationShips;
                availableColonizationShips = value;
                if (diff == 0) return; 
                var symbol = diff > 0 ? "+" : "";
                var hudView = GetView<HUDView>();
                hudView.callColonizeShipsChangedPanel($"Available colonization ships\n({symbol}{diff})");
            }
        }
        public Color Color { get; set; }

        private HeroMapView _heroMapView;
        public HeroMapView HeroMapView 
        {
            get => _heroMapView;
            set
            {
                _heroMapView = value;
                _heroMapView.Planets = _heroMapView.Planets.DistinctBy(p => p.Id).ToList();
                _heroMapView.Connections = _heroMapView.Connections.DistinctBy(p => p.Id).ToList();
            }
        }

        public Guid CapitalPlanetId { get; set; }
        private T GetView<T>() where T : Component
        {
            return UnityEngine.Object.FindFirstObjectByType<T>() ?? throw new Exception();
        }
    }
}