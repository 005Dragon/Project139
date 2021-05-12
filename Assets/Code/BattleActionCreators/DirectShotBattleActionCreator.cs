using Code.BattleActions;
using UnityEngine;

namespace Code.BattleActionCreators
{
    public class DirectShotBattleActionCreator : BattleActionCreatorBase
    {
        [SerializeField] private float _damage;

        public override BattleAction Create() => new DirectShotBattleAction(this, _damage);
    }
}