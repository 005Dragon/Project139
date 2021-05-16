using System.Collections.Generic;

namespace Code.Battle.ActionCreators
{
    public interface ITargetBattleActionCreator : IBattleActionCreator
    {
        public void SetTargets(IEnumerable<IBattleZoneField> targets);

        public IEnumerable<IBattleZoneField> GetEnableTargets(IBattleZone battleZone);
    }
}