using System;
using System.Collections.Generic;
using Code.Utils;

namespace Code.Battle.Core.UI
{
    public interface IUiBattleActionBar
    {
        bool Visible { get; set; }
        
        event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        void AddEnableBattleActionCreators(PlayerSide playerSide, IEnumerable<IBattleActionCreator> battleActionCreators);
    }
}