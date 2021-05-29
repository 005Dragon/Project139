using UnityEngine;

namespace Code.Battle
{
    public class BattleReferenceItems : IBattleReferenceItems
    {
        public Camera Camera { get; set; }
        
        public BattleSettings BattleSettings { get; set; }
        
        public BattleZoneDescription BattleZoneDescription { get; set; }
        
        public ShipController[] ShipControllers { get; set; }
    }
}