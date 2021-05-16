using System.Collections.Generic;

namespace Code.Battle
{
    public interface IBattleZone
    {
        IEnumerable<IBattleZoneField> GetBattleZoneFields();
        
        bool TryGetRelativeBattleZoneFieldByDirection(PlayerSide playerSide, Direction4 direction, out IBattleZoneField resultField);

        IBattleZoneField GetShipBattleZoneField(PlayerSide playerSide);
    }
}