using System;

namespace Code.Battle.Core
{
    public interface IBattleZoneField : IEquatable<IBattleZoneField>
    {
        PlayerSide PlayerSide { get; }
        
        int Index { get; }
        
        bool TryGetShip(out IBattleShip ship);
    }
}