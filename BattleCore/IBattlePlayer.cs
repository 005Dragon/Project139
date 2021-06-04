using System;
using System.Collections.Generic;
using BattleCore.Utils;

namespace BattleCore
{
    public interface IBattlePlayer
    {
        PlayerSide PlayerSide { get; }
        
        bool IsReady { get; }

        event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        event EventHandler Ready;

        void AddEnableBattleActionCreators(IEnumerable<IBattleActionCreator> battleActionCreators);
        
        void Sleep();

        void Wake();
    }
}