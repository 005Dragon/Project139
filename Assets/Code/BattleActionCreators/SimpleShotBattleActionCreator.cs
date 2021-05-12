using System.Collections.Generic;
using System.Linq;
using Code.BattleActions;
using UnityEngine;

namespace Code.BattleActionCreators
{
    public class SimpleShotBattleActionCreator : BattleActionCreatorBase, ITargetBattleActionCreator
    {
        [SerializeField]
        public int _damage;
        
        private Transform _target;
        
        public override BattleAction Create() => new SimpleShotBattleAction(this, _target, _damage);
        
        public IEnumerable<Transform> GetEnableTargets(BattleZoneDescription battleZoneDescription)
        {
            foreach (PlayerBattleZoneDescription playerBattleZoneDescription in battleZoneDescription.GetPlayerBattleZoneDescriptions())
            {
                if (playerBattleZoneDescription.PlayerId != Player)
                {
                    yield return playerBattleZoneDescription.BattleZone;
                }
            }
        }

        public void SetTargets(IEnumerable<Transform> targets)
        {
            _target = targets.First();
        }
    }
}