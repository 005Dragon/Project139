using BattleCore;
using Code.Battle;

namespace Code.Services
{
    public interface IBattleSettingsService
    {
        PlayerManagementType GetPlayerManagementType(PlayerSide playerSide);
        
        bool TryGetManagedPlayer(out PlayerSide playerSide);
    }
}