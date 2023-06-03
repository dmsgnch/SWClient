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

		public void Attack()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void Defence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void Research()
        {
            _planetsViewModel.ExecuteNextAction();
        }

		public void Colonize()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void BuiltLightDefence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void BuiltMidleDefence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void BuiltStrongDefence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		protected override void OnBind(PlanetsViewModel model)
        {
			_planetsViewModel = model;
        }
    }
}
