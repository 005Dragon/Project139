using BattleCore;
using UnityEngine;

namespace Code.Battle
{
    public class BattleZoneField : IBattleZoneField
    {
        public PlayerSide PlayerSide { get; }
        
        public int Index { get; }
        
        public Transform Transform { get; }

        public BattleZoneField(PlayerSide playerSide, int index, Transform transform)
        {
            PlayerSide = playerSide;
            Index = index;
            Transform = transform;
        }

        public bool TryGetShip(out IBattleShip ship)
        {
            ship = default;
            
            if (Transform.childCount == 0)
            {
                return false;
            }
            
            ship = Transform.GetComponentInChildren<ShipController>();

            return ship != null;
        }

        public bool Equals(IBattleZoneField other)
        {
            if (other == null)
            {
                return false;
            }

            return other.PlayerSide == PlayerSide && other.Index == Index;
        }
    }
}