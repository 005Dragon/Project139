using System;
using Code.Battle;
using UnityEngine;

namespace Code.Services.Controllers
{
    public class BattleSettingsServiceController : MonoBehaviour, IBattleSettingsService
    {
        [SerializeField]
        private PlayerManagementType _leftPlayerManagementType;

        [SerializeField] 
        private PlayerManagementType _rightPlayerManagementType;

        public PlayerManagementType GetPlayerManagementType(PlayerSide playerSide)
        {
            switch (playerSide)
            {
                case PlayerSide.Left: return _leftPlayerManagementType;
                case PlayerSide.Right: return  _rightPlayerManagementType;
                default: throw new ArgumentOutOfRangeException(nameof(playerSide), playerSide, null);
            }
        }

        public bool TryGetManagedPlayer(out PlayerSide playerSide)
        {
            if (_leftPlayerManagementType == PlayerManagementType.Manual)
            {
                playerSide = PlayerSide.Left;

                return true;
            }

            if (_rightPlayerManagementType == PlayerManagementType.Manual)
            {
                playerSide = PlayerSide.Right;

                return true;
            }

            playerSide = default;
            
            return false;
        }
    }
}