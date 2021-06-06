﻿using BattleCore.Log;

namespace BattleCore.Actions
{
    public class WaitAction : BattleAction
    {
        public WaitAction(PlayerSide playerSide, IBattleActionCreator creator) : base(playerSide, creator)
        {
        }

        protected override void PlayCore()
        {
            Logger.LogActionMessage(BattleLoggerMessageType.Info, this, string.Empty);
            
            Finish();
        }
    }
}