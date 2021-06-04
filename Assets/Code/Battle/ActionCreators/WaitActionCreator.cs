using Code.Battle.Core.Actions;

namespace Code.Battle.ActionCreators
{
    public class WaitActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.Wait;
        
        public override BattleAction Create(PlayerSide playerSide) => new WaitAction(playerSide, this);
    }
}