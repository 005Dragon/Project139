using System;
using BattleCore.Utils;

namespace BattleCore
{
    public interface IBattleShip
    {
        event EventHandler HealthChange;
        event EventHandler EnergyChange;
        event EventHandler ShipDestroy;
        event EventHandler<EventArgs<IShotModel>> Shot;
        event EventHandler ChangeBattleZoneFinished;
        event EventHandler ShotFinished;
        
        PlayerSide PlayerSide { get; }
        
        float MaxHealth { get; }
        
        float Health { get; }
        
        float MaxEnergy { get; }
        
        float Energy { get; }
        
        bool Destroyed { get; }

        void SetEnergy(float value);

        bool TryChangeBattleZone(Direction4 direction, out IBattleZoneField resultBattleZoneField);

        void SimpleShot(IBattleZoneField target, float damage);

        void DirectionShot(IBattleZoneField target, float damage);

        void TakeDamage(float damageValue);
    }
}