using System.Collections.Generic;
using System.Linq;
using Code.Battle.Actions;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public class SimpleShotBattleActionCreator : BattleActionCreatorBase, ITargetBattleActionCreator
    {
        [SerializeField]
        public int _damage;
        
        private IBattleZoneField _target;
        
        public override BattleAction Create() => new SimpleShotBattleAction(this, _target, _damage);
        
        public IEnumerable<IBattleZoneField> GetEnableTargets(IBattleZone battleZone)
        {
            return battleZone.GetBattleZoneFields().Where(x => x.PlayerSide != PlayerSide);
        }

        public void SetTargets(IEnumerable<IBattleZoneField> targets)
        {
            _target = targets.First();
        }
    }
}