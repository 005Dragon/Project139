using System;
using System.Collections.Generic;
using Code.Utils;

namespace Code.Battle.Core.Players
{
    public class DefaultBattlePlayer : IBattlePlayer
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        public event EventHandler Ready;
        
        public PlayerSide PlayerSide { get; }

        public bool IsReady { get; private set; }

        public DefaultBattlePlayer(PlayerSide playerSide)
        {
            PlayerSide = playerSide;
        }

        public void AddEnableBattleActionCreators(IEnumerable<IBattleActionCreator> battleActionCreators)
        {
        }

        public void Sleep()
        {
            IsReady = false;
        }

        public void Wake()
        {
            IsReady = true;
            
            Ready?.Invoke(this, EventArgs.Empty);
        }
    }
}