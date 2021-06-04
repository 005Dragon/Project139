using BattleCore.Log;

namespace BattleCore.Actions
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