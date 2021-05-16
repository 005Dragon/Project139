using System;

namespace Code.Battle
{
    public interface IBattleZoneField : IEquatable<IBattleZoneField>
    {
        PlayerSide PlayerSide { get; }
        
        int Index { get; }
        
        bool TryGetShip(out IBattleShip ship);
    }
}