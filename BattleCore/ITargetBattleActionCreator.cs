using System.Collections.Generic;

namespace BattleCore
{
    public interface ITargetBattleActionCreator : IBattleActionCreator
    {
        void SetTargets(IEnumerable<IBattleZoneField> targets);

        IEnumerable<IBattleZoneField> GetEnableTargets(PlayerSide playerSide, IBattleZone battleZone);
    }
}