using System;
using Code.Battle.ActionCreators;
using Code.Utils;

namespace Code.Battle.Actions
{
    public class SimpleShotBattleAction : BattleAction, IDisposable
    {
        public IBattleZoneField ZoneField { get; }
        
        public float Damage { get; }

        public SimpleShotBattleAction(IBattleActionCreator creator, IBattleZoneField target, float damage) : base(creator)
        {
            ZoneField = target;
            Damage = damage;
        }

        protected override void PlayCore()
        {
            SelfShip.SimpleShot(ZoneField, Damage);

            SelfShip.ShotFinished += OnShipControllerShotFinished;
        }

        private void OnShipControllerShotFinished(object sender, EventArgs<ShotModel> eventArgs)
        {
            if (eventArgs.Value.Target.TryGetShip(out IBattleShip targetShip))
            {
                targetShip.TakeDamage(eventArgs.Value.Damage);
            }
        }

        public override void Dispose()
        {
            if (SelfShip != null)
            {
                SelfShip.ShotFinished -= OnShipControllerShotFinished;    
            }
        }
    }
}