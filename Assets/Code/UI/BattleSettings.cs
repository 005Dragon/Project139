using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.UI
{
    public class BattleSettings : MonoBehaviour
    {
        [Serializable]
        public class PlayerSettings
        {
            public PlayerId PlayerId;

            public PlayerManagementType PlayerManagementType;
        }

        public PlayerSettings[] PlayersSettings =
        {
            new PlayerSettings {PlayerId = PlayerId.Left}, 
            new PlayerSettings {PlayerId = PlayerId.Right}
        };

        public List<GameObject> BattleActionTemplates;

        public PlayerId? ManagedPlayer =>
            PlayersSettings.FirstOrDefault(x => x.PlayerManagementType == PlayerManagementType.Manual)?.PlayerId;
    }
}
