using BattleCore;
using BattleCore.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveDownActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.MoveDown;

        public override BattleActionType ActionType => BattleActionType.Defense;

        public override BattleAction Create(PlayerSide playerSide) => new MoveDownAction(playerSide, this);
        
        public override object Clone()
        {
            return new MoveDownActionCreator
            {
                _duration = _duration, 
                _energyCost = _energyCost, 
                Logger = Logger
            };
        }
    }
}