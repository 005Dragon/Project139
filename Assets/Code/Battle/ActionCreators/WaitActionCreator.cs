using Code.Battle.Actions;

namespace Code.Battle.ActionCreators
{
    public class WaitActionCreator : BattleActionCreatorBase
    {
        public override BattleAction Create() => new WaitAction(this);
    }
}