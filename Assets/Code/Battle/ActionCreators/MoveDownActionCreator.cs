using BattleCore;
using BattleCore.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveDownActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.MoveDown;
        
        public override BattleAction Create(PlayerSide playerSide) => new MoveDownAction(playerSide, this);
    }
}