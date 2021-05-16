using Code.Battle.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveDownActionCreator : BattleActionCreatorBase
    {
        public override BattleAction Create() => new MoveDownAction(this);
    }
}