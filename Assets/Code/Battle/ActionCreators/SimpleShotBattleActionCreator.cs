using System.Collections.Generic;
using System.Linq;
using BattleCore;
using BattleCore.Actions;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public class SimpleShotBattleActionCreator : BattleActionCreatorBase, ITargetBattleActionCreator
    {
        public override BattleActionId ActionId => BattleActionId.SimpleShot;

        public override BattleActionType ActionType => BattleActionType.Attack;

        [SerializeField]
        public int _damage;
        
        private IBattleZoneField _target;
        
        public override BattleAction Create(PlayerSide playerSide) => new SimpleShotBattleAction(playerSide, this, _target, _damage);
        public override object Clone()
        {
            return new SimpleShotBattleActionCreator
            {
                _duration = _duration, 
                _energyCost = _energyCost, 
                _damage = _damage,
                _target = _target,
                Logger = Logger
            };
        }

        public IEnumerable<IBattleZoneField> GetEnableTargets(PlayerSide playerSide, IBattleZone battleZone)
        {
            return battleZone.GetBattleZoneFields().Where(x => x.PlayerSide != playerSide);
        }

        public void SetTargets(IEnumerable<IBattleZoneField> targets)
        {
            _target = targets.First();
        }
    }
}