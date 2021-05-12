using Code.BattleActionCreators;

namespace Code.BattleActions
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