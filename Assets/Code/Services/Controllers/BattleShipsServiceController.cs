using System.Collections.Generic;
using System.Linq;
using Code.Battle;
using Code.Battle.Core;
using UnityEngine;

namespace Code.Services.Controllers
{
    public class BattleShipsServiceController : MonoBehaviour, IBattleShipsService
    {
        [SerializeField]
        private ShipController[] _shipControllers;
        
        public IEnumerable<IBattleShip> GetBattleShips()
        {
            return _shipControllers.Select(x => (IBattleShip) x);
        }

        private void Awake()
        {
            if (_shipControllers == null)
            {
                _shipControllers = GetComponentsInChildren<ShipController>();    
            }
        }
    }
}