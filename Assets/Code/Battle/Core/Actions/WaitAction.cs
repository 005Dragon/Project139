using Code.Battle.Core.Log;

namespace Code.Battle.Core.Actions
{
    public class WaitAction : BattleAction
    {
        public WaitAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
            Logger.LogActionMessage(BattleLoggerMessageType.Info, this, string.Empty);
            
            Finish();
        }
    }
}