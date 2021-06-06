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
        
        [SerializeField]
        public int _damage;
        
        private IBattleZoneField _target;
        
        public override BattleAction Create(PlayerSide playerSide) => new SimpleShotBattleAction(playerSide, this, _target, _damage);
        
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