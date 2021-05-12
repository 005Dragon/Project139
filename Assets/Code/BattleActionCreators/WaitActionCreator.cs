using Code.BattleActions;

namespace Code.BattleActionCreators
{
    public class WaitActionCreator : BattleActionCreatorBase
    {
        public override BattleAction Create() => new WaitAction(this);
    }
}