using System;
using System.Collections.Generic;
using Code.Utils;

namespace Code.Battle.Core
{
    public interface IBattlePlayer
    {
        public PlayerSide PlayerSide { get; }
        
        bool IsReady { get; }

        event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        event EventHandler Ready;

        void AddEnableBattleActionCreators(IEnumerable<IBattleActionCreator> battleActionCreators);
        
        void Sleep();

        void Wake();
    }
}