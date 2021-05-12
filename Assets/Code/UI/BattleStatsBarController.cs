using Code.UI.ProgressBar;
using UnityEngine;

namespace Code.UI
{
    public class BattleStatsBarController : MonoBehaviour
    {
        public PlayerId Player;

        [SerializeField] 
        private ProgressBarController _healthBar;

        [SerializeField]
        private ProgressBarController _EnergyBar;

        public void Build(ShipController shipController)
        {
            _healthBar.Build(shipController.Heath);
            _EnergyBar.Build(shipController.Energy);
        }

        public void SetHealth(float value)
        {
            _healthBar.SetValue(value);
        }

        public void SetEnergy(float value)
        {
            _EnergyBar.SetValue(value);
        }
    }
}