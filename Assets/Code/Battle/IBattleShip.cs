using System;
using Code.Utils;

namespace Code.Battle
{
    public interface IBattleShip
    {
        event EventHandler<EventArgs<ShotModel>> ShotFinished;
        
        PlayerSide PlayerSide { get; }
        
        bool ActionInProcess { get; }
        
        float Energy { get; }

        void SetEnergy(float value);

        void ChangeBattleZone(Direction4 direction);

        void SimpleShot(IBattleZoneField target, float damage);

        void DirectionShot(IBattleZoneField target, float damage);

        void TakeDamage(float damageValue);
    }
}