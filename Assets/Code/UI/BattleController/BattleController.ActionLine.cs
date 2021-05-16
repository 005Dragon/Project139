using System;
using System.Linq;
using Code.Battle.Actions;
using Code.Utils;

namespace Code.UI.BattleController
{
    public partial class BattleController
    {
        private BattleActionLineUiController _battleActionLineUiController;
        
        private void InitializeActionLineUiController()
        {
            _battleActionLineUiController = GetComponentInChildren<BattleActionLineUiController>();
            _battleActionLineUiController.Initialize(ReferenceItems);
            _battleActionLineUiController.BeginPlay += OnBattleActionLineUiControllerBeginPlay;
            _battleActionLineUiController.EndPlay += OnBattleActionLineUiControllerEndPlay;
            _battleActionLineUiController.PlayerReady += OnOnBattleActionLineUiControllerPlayerReady;
            _battleActionLineUiController.CancelAction += OnBattleActionLineUiControllerCancelAction;

            _uis.Add(_battleActionLineUiController);
        }

        private void OnOnBattleActionLineUiControllerPlayerReady(object sender, EventArgs<PlayerSide> eventArgs)
        {
            Ready(eventArgs.Value);
        }

        private void OnBattleActionLineUiControllerBeginPlay(object sender, EventArgs eventArgs)
        {
            SetActiveUi(_battleActionLineUiController);
        }

        private void OnBattleActionLineUiControllerEndPlay(object sender, EventArgs eventArgs)
        {
            foreach (ShipController shipController in ReferenceItems.ShipControllers)
            {
                shipController.SetEnergy(shipController.MaxEnergy);
            }
            
            Reset();
        }

        private void OnBattleActionLineUiControllerCancelAction(object sender, EventArgs<BattleAction> eventArgs)
        {
            BattleAction battleAction = eventArgs.Value;

            ShipController shipController = ReferenceItems.ShipControllers.First(x => x.PlayerSide == battleAction.PlayerSide);
            shipController.SetEnergy(shipController.Energy + battleAction.EnergyCost);
        }
    }
}