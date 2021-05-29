﻿using Code.Battle.Core;
using Code.UI.ProgressBar;
using UnityEngine;

namespace Code.UI
{
    public class BattleStatsBarController : MonoBehaviour
    {
        public PlayerSide PlayerSide;

        [SerializeField] 
        private ProgressBarController _healthBar;

        [SerializeField]
        private ProgressBarController _EnergyBar;

        public void Build(float maxHealth, float maxEnergy)
        {
            _healthBar.Build(maxHealth);
            _EnergyBar.Build(maxEnergy);
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