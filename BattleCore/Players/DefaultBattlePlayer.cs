using System;
using BattleCore.Utils;

namespace BattleCore.Players
{
    public class DefaultBattlePlayer : BattlePlayerBase
    {
        public override event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        public DefaultBattlePlayer(PlayerSide playerSide) : base(playerSide)
        {
        }

        public override void AddEnableBattleActionCreators(IBattleActionCreator[] battleActionCreators)
        {
        }

        public override void Wake()
        {
            IsReady = true;
        }
    }
}