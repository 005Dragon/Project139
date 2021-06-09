using BattleCore.Log;

namespace BattleCore.Actions
{
    public class WaitAction : BattleAction
    {
        public WaitAction(PlayerSide playerSide, IBattleActionCreator creator) : base(playerSide, creator)
        {
        }

        protected override void PlayCore()
        {
            Logger.LogAction(this, string.Empty);
            
            Finish();
        }
    }
}