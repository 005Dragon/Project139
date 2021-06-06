using System;
using BattleCore.Log;
using BattleCore.Utils;

namespace BattleCore.Actions
{
    public class DirectShotBattleAction : BattleAction
    {
        public float Damage { get; }

        private float _totalDamage;
        
        public DirectShotBattleAction(PlayerSide playerSide, IBattleActionCreator creator, float damage) : base(playerSide, creator)
        {
            Damage = damage;
        }

        protected override void PlayCore()
        {
            BattleZone.TryGetRelativeBattleZoneFieldByDirection(
                PlayerSide,
                PlayerSide == PlayerSide.Left ? Direction4.Right : Direction4.Left,
                out IBattleZoneField target
            );

            SelfShip.Shot += OnSelfShipShot;
            SelfShip.ShotFinished += OnSelfShipShotFinished;
            
            SelfShip.DirectionShot(target, Damage);
        }

        private void OnSelfShipShot(object sender, EventArgs<ShotModel> eventArgs)
        {
            if (eventArgs.Value.Target.TryGetShip(out IBattleShip ship))
            {
                _totalDamage += eventArgs.Value.Damage;
                
                ship.TakeDamage(eventArgs.Value.Damage);
            }
        }
        
        private void OnSelfShipShotFinished(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;

            ship.Shot -= OnSelfShipShot;
            ship.ShotFinished -= OnSelfShipShotFinished;

            Logger.LogActionMessage(BattleLoggerMessageType.Info, this, _totalDamage + " damage.");
            
            Finish();
        }
    }
}