using System;
using Code.BattleActionCreators;

namespace Code.UI
{
    public class CreateBattleActionEventArgs : EventArgs
    {
        public PlayerId Player { get; }
        
        public IBattleActionCreator BattleActionCreator { get; }

        public CreateBattleActionEventArgs(PlayerId player, IBattleActionCreator battleActionCreator)
        {
            BattleActionCreator = battleActionCreator;
        }
    }
}