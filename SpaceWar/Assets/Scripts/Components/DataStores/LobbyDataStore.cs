using Assets.Scripts.Components.Abstract;
using SharedLibrary.Models;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class LobbyDataStore : IDataStore
    {
        /// <summary>
        /// Id of the lobby we are in
        /// </summary>
        public Guid LobbyId { get; set; }
        public LobbyInfo LobbyInfo { get; set; }
    }
}