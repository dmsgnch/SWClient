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
        private PlanetsViewModel _viewModel;

        [SerializeField] private GameObject planetsParent;
        [SerializeField] private GameObject connectionsParent;
        [SerializeField] private GameObject dropdownPrefab;
        [SerializeField] private GameObject[] planetPrefabs;

        public void GeneratePlanetsWithConnections()
        {
            GameObject[] planets = _viewModel.GeneratePlanets(planetPrefabs, planetsParent, dropdownPrefab);
            _viewModel.CreateConnections(connectionsParent, planets);
        }

        private void Awake()
        {
            if (_viewModel is null) return;

        }

        protected override void OnBind(PlanetsViewModel model)
        {
            _viewModel = model;
        }
    }
}
