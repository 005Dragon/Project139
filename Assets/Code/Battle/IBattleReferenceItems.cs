using UnityEngine;

namespace Code.Battle
{
    public interface IBattleReferenceItems
    {
        Camera Camera { get; }
        
        BattleSettings BattleSettings { get; }
        
        BattleZoneDescription BattleZoneDescription { get; }
        
        ShipController[] ShipControllers { get; }
    }
}