using System;
using System.Collections.Generic;
using BattleCore.Utils;

namespace BattleCore.UI
{
    public interface IUiBattleActionBar
    {
        bool Visible { get; set; }
        
        event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        void AddEnableBattleActionCreators(PlayerSide playerSide, IEnumerable<IBattleActionCreator> battleActionCreators);
    }
}