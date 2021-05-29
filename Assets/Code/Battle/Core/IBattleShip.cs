using System;
using Code.Utils;

namespace Code.Battle.Core
{
    public interface IBattleShip
    {
        public event EventHandler HealthChange;
        public event EventHandler EnergyChange;
        event EventHandler ShipDestroy;
        event EventHandler<EventArgs<ShotModel>> Shot;
        event EventHandler ChangeBattleZoneFinished;
        event EventHandler ShotFinished;
        
        PlayerSide PlayerSide { get; }
        
        float MaxHealth { get; }
        
        float Health { get; }
        
        float MaxEnergy { get; }
        
        float Energy { get; }

        void SetEnergy(float value);

        bool TryChangeBattleZone(Direction4 direction, out IBattleZoneField resultBattleZoneField);

        void SimpleShot(IBattleZoneField target, float damage);

        void DirectionShot(IBattleZoneField target, float damage);

        void TakeDamage(float damageValue);
    }
}