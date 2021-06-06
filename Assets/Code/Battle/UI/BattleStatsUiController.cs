using System.Linq;
using BattleCore;
using Code.UI;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleStatsUiController : MonoBehaviour
    {
        private BattleStatsBarController[] _statsBarControllers;

        public void SetHealth(PlayerSide playerSide, float value)
        {
            _statsBarControllers.First(x => x.PlayerSide == playerSide).SetHealth(value);
        }

        public void SetEnergy(PlayerSide playerSide, float value)
        {
            _statsBarControllers.First(x => x.PlayerSide == playerSide).SetEnergy(value);
        }

        public void Build(IBattleShip[] ships)
        {
            foreach (BattleStatsBarController statsBarController in _statsBarControllers)
            {
                IBattleShip ship = ships.First(x => x.PlayerSide == statsBarController.PlayerSide);
                
                statsBarController.Build(ship.MaxHealth, ship.MaxEnergy);
            }
        }

        private void Awake()
        {
            _statsBarControllers = GetComponentsInChildren<BattleStatsBarController>();
        }
    }
}