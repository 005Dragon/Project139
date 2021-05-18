using System;

namespace Code.UI.BattleUiController
{
    public partial class BattleUiController
         {
             private BattleSelectTargetUiController _battleSelectTargetUiController;
             
             private void InitializeSelectTargetUiController()
             {
                 _battleSelectTargetUiController = GetComponentInChildren<BattleSelectTargetUiController>();
                 _battleSelectTargetUiController.Initialize(ReferenceItems);
                 _battleSelectTargetUiController.Cancel += OnSelectTargetCancel;
                 _battleSelectTargetUiController.Apply += OnSelectTargetApply;
                 
                 _uis.Add(_battleSelectTargetUiController);
             }
     
             private void OnSelectTargetApply(object sender, BattleSelectTargetUiController.ApplyEventArgs eventArgs)
             {
                 CreateBattleAction(eventArgs.TargetBattleActionCreator);
                 
                 SetActiveUi(_battleMainUiController);
             }
     
             private void OnSelectTargetCancel(object sender, EventArgs eventArgs)
             {
                 SetActiveUi(_battleMainUiController);
             }
         }
}