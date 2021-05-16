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
            public PlayerSide playerSide;

            public PlayerManagementType PlayerManagementType;
        }

        public PlayerSettings[] PlayersSettings =
        {
            new PlayerSettings {playerSide = PlayerSide.Left}, 
            new PlayerSettings {playerSide = PlayerSide.Right}
        };

        public List<GameObject> BattleActionTemplates;

        public PlayerSide? ManagedPlayer =>
            PlayersSettings.FirstOrDefault(x => x.PlayerManagementType == PlayerManagementType.Manual)?.playerSide;
    }
}
