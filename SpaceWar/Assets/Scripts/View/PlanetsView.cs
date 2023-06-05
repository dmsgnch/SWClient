using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using Components;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

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

        public async void GeneratePlanetsWithConnections()
        {
            _planetsViewModel.GeneratePlanets(GetPlanetsGenerationForm());
            await Task.Delay((int)Time.deltaTime*1000);
            _planetsViewModel.CreateConnections(connectionsParent);
        }

        public void UpdatePlanet(Planet planet)
        {
            _planetsViewModel.UpdatePlanet(planet,GetPlanetsGenerationForm());
        }

        public void UpdateConnection(Edge connection)
        {
            _planetsViewModel.UpdateConnection(connection, connectionsParent);
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
            await _planetsViewModel.ResearchOrColonizeRequest(planet);
        }

		public async void Colonize(Planet planet)
		{
			await _planetsViewModel.ResearchOrColonizeRequest(planet);
		}

		public async void BuildDefence(Planet planet)
		{
			await _planetsViewModel.BuildDefence(planet);
        }

        private PlanetsGenerationForm GetPlanetsGenerationForm()
        {
            return new PlanetsGenerationForm
            {
                PlanetPrefabs = planetPrefabs,
                PlanetsParent = planetsParent,
                PlanetInfoPanelPrefab = PlanetInfoPanelPrefab,
                PlanetIconsPrefabs = planetIconsPrefabs,
                ButtonPrefab = ButtonPrefab,
                PlanetTextPrefab = textPrefab,
                HealthbarPrefab = healthbarPrefab,
            };
        }

		protected override void OnBind(PlanetsViewModel model)
        {
			_planetsViewModel = model;
        }
    }
}
