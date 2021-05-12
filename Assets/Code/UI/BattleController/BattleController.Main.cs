using System.Linq;
using Code.BattleActionCreators;
using Code.BattleActions;

namespace Code.UI.BattleController
{
    public partial class BattleController
    {
        private BattleMainUiController _battleMainUiController;
        
        private void InitializeMainUiController()
        {
            _battleMainUiController = GetComponentInChildren<BattleMainUiController>();
            _battleMainUiController.Initialize(ReferenceItems);
            _battleMainUiController.CreateBattleAction += OnCreateBattleAction;

            _uis.Add(_battleMainUiController);
        }

        private void OnCreateBattleAction(object sender, CreateBattleActionEventArgs eventArgs)
        {
            if (!CanCreateBattleAction(eventArgs.BattleActionCreator))
            {
                return;
            }
            
            if (eventArgs.BattleActionCreator is ITargetBattleActionCreator targetBattleActionCreator)
            {
                _battleSelectTargetUiController.SetTargetBattleActionCreator(targetBattleActionCreator);
                
                SetActiveUi(_battleSelectTargetUiController);
                
                return;
            }
            
            CreateBattleAction(eventArgs.BattleActionCreator);
        }

        private void CreateBattleAction(IBattleActionCreator creator)
        {
            BattleAction battleAction = creator.Create();

            ShipController shipController = ReferenceItems.ShipControllers.First(x => x.Player == battleAction.Player);
            shipController.SetEnergy(shipController.Energy - battleAction.EnergyCost);
            
            _battleActionLineUiController.AddBattleAction(battleAction);
        }

        private bool CanCreateBattleAction(IBattleActionCreator creator)
        {
            float energyCost = creator.EnergyCost;

            ShipController shipController = ReferenceItems.ShipControllers.First(x => x.Player == creator.Player);

            return energyCost <= shipController.Energy;
        }
    }
}