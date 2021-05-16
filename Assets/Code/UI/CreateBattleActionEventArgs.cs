using System;
using Code.Battle.ActionCreators;

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