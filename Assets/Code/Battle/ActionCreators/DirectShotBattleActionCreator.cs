using Code.Battle.Core.Actions;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public class DirectShotBattleActionCreator : BattleActionCreatorBase
    {
        public override BattleActionId ActionId => BattleActionId.DirectShot;
        
        [SerializeField] private float _damage;
        
        public override BattleAction Create(PlayerSide playerSide) => new DirectShotBattleAction(playerSide, this, _damage);
    }
}