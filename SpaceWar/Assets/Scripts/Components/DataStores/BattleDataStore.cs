using Assets.Scripts.Components.Abstract;
using Assets.Scripts.View;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.DataStores
{
	public class BattleDataStore : IDataStore
	{
		/// <summary>
		/// List of battles
		/// </summary>
		public List<Battle> Battles { get; set; }

	}
}
