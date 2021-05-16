using UnityEngine;

namespace Code.UI
{
    public class BattleReferenceItems : IBattleReferenceItems
    {
        public Camera Camera { get; set; }
        
        //public IBattleActionCreator BattleActionCreator { get; set; }

        public BattleSettings BattleSettings { get; set; }
        
        public BattleZoneDescription BattleZoneDescription { get; set; }
        
        public ShipController[] ShipControllers { get; set; }

        // public void Reset()
        // {
        //     BattleActionCreator = null;
        // }
    }
}