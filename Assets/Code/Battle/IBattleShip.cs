using System;
using Code.Utils;

namespace Code.Battle
{
    public interface IBattleShip
    {
        event EventHandler ShipDestroy;
        event EventHandler<EventArgs<ShotModel>> Shot;
        event EventHandler ChangeBattleZoneFinished;
        event EventHandler ShotFinished;
        
        PlayerSide PlayerSide { get; }
        
        float Energy { get; }

        void SetEnergy(float value);

        bool TryChangeBattleZone(Direction4 direction, out IBattleZoneField resultBattleZoneField);

        void SimpleShot(IBattleZoneField target, float damage);

        void DirectionShot(IBattleZoneField target, float damage);

        void TakeDamage(float damageValue);
    }
}