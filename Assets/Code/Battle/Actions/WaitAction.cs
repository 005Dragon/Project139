using Code.Battle.ActionCreators;

namespace Code.Battle.Actions
{
    public class WaitAction : BattleAction
    {
        public WaitAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
        }
    }
}