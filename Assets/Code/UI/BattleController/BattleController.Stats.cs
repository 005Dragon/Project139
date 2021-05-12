﻿using System;

namespace Code.UI.BattleController
{
    public partial class BattleController
    {
        private BattleStatsUiController _battleStatsUiController;
             
        private void InitializeStatsUiController()
        {
            _battleStatsUiController = GetComponentInChildren<BattleStatsUiController>();
            _battleStatsUiController.Initialize(ReferenceItems);

            foreach (ShipController shipController in ReferenceItems.ShipControllers)
            {
                shipController.HealthChange += OnShipControllerHealthChange;
                shipController.EnergyChange += OnShipControllerEnergyChange;
            }
                 
            _uis.Add(_battleStatsUiController);
        }
        
        private void OnShipControllerHealthChange(object sender, EventArgs e)
        {
            var shipController = (ShipController)sender;

            BattleStatsBarController statsController = _battleStatsUiController.GetStats(shipController.Player);
            
            statsController.SetHealth(shipController.Heath);
        }
        
        private void OnShipControllerEnergyChange(object sender, EventArgs e)
        {
            var shipController = (ShipController)sender;

            BattleStatsBarController statsController = _battleStatsUiController.GetStats(shipController.Player);
            
            statsController.SetEnergy(shipController.Energy);
        }
    }
}