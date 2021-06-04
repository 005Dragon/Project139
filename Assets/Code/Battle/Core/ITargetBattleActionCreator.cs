using System.Collections.Generic;

namespace Code.Battle.Core
{
    public interface ITargetBattleActionCreator : IBattleActionCreator
    {
        public void SetTargets(IEnumerable<IBattleZoneField> targets);

        public IEnumerable<IBattleZoneField> GetEnableTargets(PlayerSide playerSide, IBattleZone battleZone);
    }
}