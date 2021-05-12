namespace Code.UI.BattleController
{
    public partial class BattleController
    {
        private BattleResultUiController _battleResultUiController;
        
        private void InitializeResultUiController()
        {
            _battleResultUiController = GetComponentInChildren<BattleResultUiController>();
            _battleResultUiController.Initialize(ReferenceItems);

            _uis.Add(_battleResultUiController);
        }
    }
}