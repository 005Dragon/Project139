using System;
using BattleCore.Log;
using BattleCore.Utils;

namespace BattleCore.Actions
{
    public class SimpleShotBattleAction : BattleAction
    {
        public IBattleZoneField ZoneField { get; }
        
        public float Damage { get; }

        private float _totalDamage;

        public SimpleShotBattleAction(PlayerSide playerSide, IBattleActionCreator creator, IBattleZoneField target, float damage) 
            : base(playerSide, creator)
        {
            ZoneField = target;
            Damage = damage;
        }

        protected override void PlayCore()
        {
            SelfShip.Shot += OnSelfShipShot;
            SelfShip.ShotFinished += OnSelfShipShotFinished;
            
            SelfShip.SimpleShot(ZoneField, Damage);
        }

        private void OnSelfShipShot(object sender, EventArgs<IShotModel> eventArgs)
        {
            if (eventArgs.Value.Target.TryGetShip(out IBattleShip targetShip))
            {
                _totalDamage += eventArgs.Value.Damage;
                
                targetShip.TakeDamage(eventArgs.Value.Damage);
            }
        }
        
        private void OnSelfShipShotFinished(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;

            ship.Shot -= OnSelfShipShot;
            ship.ShotFinished -= OnSelfShipShotFinished;

            Logger.LogAction(this, _totalDamage + " damage.");
            
            Finish();
        }
    }
}