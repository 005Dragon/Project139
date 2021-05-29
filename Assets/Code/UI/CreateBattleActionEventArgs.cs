using System;
using Code.Battle.ActionCreators;
using Code.Battle.Core;

namespace Code.UI
{
    public class CreateBattleActionEventArgs : EventArgs
    {
        public PlayerSide Player { get; }
        
        public IBattleActionCreator BattleActionCreator { get; }

        public CreateBattleActionEventArgs(PlayerSide player, IBattleActionCreator battleActionCreator)
        {
            BattleActionCreator = battleActionCreator;
        }
    }
}