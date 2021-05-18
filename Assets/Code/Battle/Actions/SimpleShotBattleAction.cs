﻿using System;
using Code.Battle.ActionCreators;
using Code.Battle.Log;
using Code.Utils;

namespace Code.Battle.Actions
{
    public class SimpleShotBattleAction : BattleAction
    {
        public IBattleZoneField ZoneField { get; }
        
        public float Damage { get; }

        private float _totalDamage;

        public SimpleShotBattleAction(IBattleActionCreator creator, IBattleZoneField target, float damage) : base(creator)
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

        private void OnSelfShipShot(object sender, EventArgs<ShotModel> eventArgs)
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

            Logger.LogActionMessage(BattleLoggerMessageType.Info, this, _totalDamage + " damage.");
            
            Finish();
        }
    }
}