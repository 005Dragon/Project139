using Code.Battle.Core.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveDownActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.MoveDown;
        
        public override BattleAction Create() => new MoveDownAction(this);
    }
}