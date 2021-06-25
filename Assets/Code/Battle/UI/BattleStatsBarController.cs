using BattleCore;
using Code.UI.ProgressBar;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Battle.UI
{
    public class BattleStatsBarController : MonoBehaviour
    {
        public PlayerSide PlayerSide;

        [SerializeField] 
        private Text _healthText;
        
        [SerializeField] 
        private ProgressBarUiController healthBarUi;

        [SerializeField]
        private Text _energyText;
        
        [SerializeField]
        private ProgressBarUiController energyBarUi;

        private string _healthTextTemplate;
        private string _energyTextTemplate;
        
        private void Awake()
        {
            _healthTextTemplate = _healthText.text;
            _energyTextTemplate = _energyText.text;
        }

        public void SetHealth(float value, float maxValue)
        {
            healthBarUi.SetValue(value, maxValue);
            
            SetHealthText(value);
        }
        
        public void SetHealth(float value)
        {
            healthBarUi.SetValue(value);
            
            SetHealthText(value);
        }

        public void SetEnergy(float value, float maxValue)
        {
            energyBarUi.SetValue(value, maxValue);
            
            SetEnergyText(value);
        }

        public void SetEnergy(float value)
        {
            energyBarUi.SetValue(value);
            
            SetEnergyText(value);
        }

        private void SetHealthText(float value)
        {
            _healthText.text = $"{_healthTextTemplate} {value:N1}";
        }
        
        private void SetEnergyText(float value)
        {
            _energyText.text = $"{_energyTextTemplate} {value:N1}";
        }
    }
}