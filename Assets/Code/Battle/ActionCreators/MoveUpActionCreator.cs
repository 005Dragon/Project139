using BattleCore;
using BattleCore.Actions;

namespace Code.Battle.ActionCreators
{
    public class MoveUpActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.MoveUp;
        
        public override BattleActionType ActionType => BattleActionType.Defense;

        public override BattleAction Create(PlayerSide playerSide) => new MoveUpAction(playerSide, this);
        public override object Clone()
        {
            return new MoveUpActionCreator
            {
                _duration = _duration, 
                _energyCost = _energyCost, 
                Logger = Logger
            };
        }
    }
}