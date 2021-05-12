using Code.BattleActionCreators;
using UnityEngine;

namespace Code.UI
{
    public interface IBattleReferenceItems
    {
        Camera Camera { get; }
        
        BattleSettings BattleSettings { get; }
        
        BattleZoneDescription BattleZoneDescription { get; }
        
        ShipController[] ShipControllers { get; }
    }
}