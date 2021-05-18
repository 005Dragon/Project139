using System;
using Code.Battle.ActionCreators;
using Code.Battle.Log;

namespace Code.Battle.Actions
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