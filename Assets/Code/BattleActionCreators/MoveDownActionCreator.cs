using Code.BattleActions;

namespace Code.BattleActionCreators
{
    public class MoveDownActionCreator : BattleActionCreatorBase
    {
        public override BattleAction Create() => new MoveDownAction(this);
    }
}