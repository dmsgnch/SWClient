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

		#region Commands

		public void GeneratePlanetsWithConnections()
        {
            _planetsViewModel.GeneratePlanets(GetPlanetsGenerationForm());

            _planetsViewModel.CreateConnections(connectionsParent);
        }

        public void UpdatePlanet(Planet planet)
        {
            _planetsViewModel.UpdatePlanet(planet,GetPlanetsGenerationForm(),connectionsParent);
        }

        public void UpdateConnection(Edge connection)
        {
            _planetsViewModel.UpdateConnection(connection, connectionsParent);
        }

        public void AddBattle(Battle battle) {
            _planetsViewModel.AddBattleToPlanet(GetPlanetsGenerationForm(), battle);
        }

        public async void Attack(Planet planet)
		{
			await _planetsViewModel.Attack(planet);
		}

		public async void Defence(Planet planet)
		{
			await _planetsViewModel.Defend(planet);
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

		#endregion

		#region Helpers

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

        #endregion

        protected override void OnBind(PlanetsViewModel model)
        {
			_planetsViewModel = model;
        }
    }
}
