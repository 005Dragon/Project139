using Code.Battle.Core.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveUpActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.MoveUp;
        
        public override BattleAction Create() => new MoveUpAction(this);
    }
}