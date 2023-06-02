using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
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

        public void GeneratePlanetsWithConnections()
        {
            GameObject[] planets = _planetsViewModel.GeneratePlanets(planetPrefabs, planetsParent, dropdownPrefab, planetIconsPrefabs, ButtonPrefab);
            _planetsViewModel.CreateConnections(connectionsParent, planets);
        }

        private void Awake()
        {
            if (_planetsViewModel is null) return;

        }

  //      public GameObject GetPrefabByPlanetType()
  //      {
  //          return _planetsViewModel.GetPrefabByPlanetType();
		//}

        protected override void OnBind(PlanetsViewModel model)
        {
			_planetsViewModel = model;
        }
    }
}
