using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    public class PlanetsGenerationForm
    {
        public GameObject[] PlanetPrefabs { get;set; }
        public GameObject PlanetsParent { get; set; }
        public GameObject PlanetInfoPanelPrefab { get; set; }
        public GameObject[] PlanetIconsPrefabs { get; set; }
        public GameObject ButtonPrefab { get; set; }
        public GameObject PlanetTextPrefab { get; set; }
        public GameObject HealthbarPrefab { get; set; }
    }
}
