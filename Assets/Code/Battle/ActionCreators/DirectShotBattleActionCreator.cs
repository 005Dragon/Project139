using BattleCore;
using BattleCore.Actions;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public class DirectShotBattleActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.DirectShot;

        public override BattleActionType ActionType => BattleActionType.Attack;

        [SerializeField] private float _damage;
        
        public override BattleAction Create(PlayerSide playerSide) => new DirectShotBattleAction(playerSide, this, _damage);
        
        public override object Clone()
        {
            return new DirectShotBattleActionCreator
            {
                _duration = _duration, 
                _energyCost = _energyCost, 
                _damage = _damage, 
                Logger = Logger
            };
        }
    }
}