using Code.Battle.Actions;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public class DirectShotBattleActionCreator : BattleActionCreatorBase
    {
        [SerializeField] private float _damage;

        public override BattleAction Create() => new DirectShotBattleAction(this, _damage);
    }
}