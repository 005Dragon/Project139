using System.Collections.Generic;
using UnityEngine;

namespace Code.BattleActionCreators
{
    public interface ITargetBattleActionCreator : IBattleActionCreator
    {
        public void SetTargets(IEnumerable<Transform> targets);

        public IEnumerable<Transform> GetEnableTargets(BattleZoneDescription battleZoneDescription);
    }
}