using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class PlanetsView : AbstractScreen<PlanetsViewModel>
    {
        private PlanetsViewModel _planetsViewModel;

        [SerializeField] private GameObject planetsParent;
        [SerializeField] private GameObject connectionsParent;
        [SerializeField] private GameObject dropdownPrefab;
        [SerializeField] private GameObject[] planetPrefabs;
        [SerializeField] private GameObject[] planetIconsPrefabs;
        [SerializeField] private GameObject ButtonPrefab;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private GameObject healthbarPrefab;
        [SerializeField] private GameObject PlanetInfoPanelPrefab;

        public void GeneratePlanetsWithConnections()
        {
            GameObject[] planets = _planetsViewModel.GeneratePlanets(planetPrefabs, 
                planetsParent, PlanetInfoPanelPrefab, planetIconsPrefabs, ButtonPrefab, textPrefab, healthbarPrefab);
            _planetsViewModel.CreateConnections(connectionsParent, planets);
        }

        private void Awake()
        {
            if (_planetsViewModel is null) return;

        }

		public async void Attack(Planet planet)
		{
			_planetsViewModel.Attack(planet);
		}

		public async void Defence(Planet planet)
		{
			_planetsViewModel.Defend(planet);
		}

		public async void Research(Planet planet)
        {
            await _planetsViewModel.Research(planet);
        }

		public async void Colonize(Planet planet)
		{
			await _planetsViewModel.Colonize(planet);
		}

		public async void BuiltLightDefence(Planet planet)
		{
			_planetsViewModel.BuiltLightDefence(planet);
		}

		public async void BuiltMidleDefence(Planet planet)
		{
			_planetsViewModel.BuiltMidleDefence(planet);
		}

		public async void BuiltStrongDefence(Planet planet)
		{
			_planetsViewModel.BuiltStrongDefence(planet);
		}

		protected override void OnBind(PlanetsViewModel model)
        {
			_planetsViewModel = model;
        }
    }
}
