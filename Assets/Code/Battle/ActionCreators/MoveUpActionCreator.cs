using Code.Battle.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveUpActionCreator : BattleActionCreatorBase
    {
        public override BattleAction Create() => new MoveUpAction(this);
    }
}