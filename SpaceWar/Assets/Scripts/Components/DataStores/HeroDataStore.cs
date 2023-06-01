using Assets.Scripts.Components.Abstract;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.DataStores
{
	public class HeroDataStore : IDataStore
	{
		/// <summary>
		/// The id of the session that we receive after start button press and use to create a requests
		/// </summary>
		public Guid HeroId { get; set; }
		public string Name { get; set; }

		public int Resourses { get; set; }
		public int SoldiersLimit { get; set; }
		public int AvailableSoldiers { get; set; }
		public byte ResearchShipLimit { get; set; }
		public byte AvailableResearchShips { get; set; } = 0;
		public byte ColonizationShipLimit { get; set; }
		public byte AvailableColonizationShips { get; set; } = 0;
		public Color Color { get; set; }
		public HeroMapView HeroMapView { get; set; }
	}
}
