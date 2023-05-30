using Assets.Scripts.Components.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Components.DataStores
{
	public class HeroDataStore : IDataStore
	{
		/// <summary>
		/// The id of the session that we receive after start button press and use to create a requests
		/// </summary>
		public Guid HeroId { get; set; }
	}
}
