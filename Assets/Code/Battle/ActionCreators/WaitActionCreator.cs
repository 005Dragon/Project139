using BattleCore;
using BattleCore.Actions;

namespace Code.Battle.ActionCreators
{
    public class WaitActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.Wait;
        public override BattleActionType ActionType => BattleActionType.Internal;

        public override BattleAction Create(PlayerSide playerSide) => new WaitAction(playerSide, this);
        public override object Clone()
        {
            return new WaitActionCreator
            {
                _duration = _duration,
                _energyCost = _energyCost,
                Logger = Logger
            };
        }
    }
}