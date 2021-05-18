namespace Code.UI.BattleUiController
{
    public partial class BattleUiController
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