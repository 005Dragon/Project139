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
        event EventHandler<EventArgs<bool>> ReadyChange;

        void AddEnableBattleActionCreators(IBattleActionCreator[] battleActionCreators);
        
        void Sleep();

        void Wake();
    }
}