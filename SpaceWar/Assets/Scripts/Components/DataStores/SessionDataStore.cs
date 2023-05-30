using Assets.Scripts.Components.Abstract;
using Assets.Scripts.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.DataStores
{
	public class SessionDataStore : IDataStore
	{
		/// <summary>
		/// The id of the session that we receive after start button press and use to create a requests
		/// </summary>
		public Guid SessionId { get; set; }
	}
}
