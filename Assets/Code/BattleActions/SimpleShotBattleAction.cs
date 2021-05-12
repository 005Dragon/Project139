using System;
using Code.BattleActionCreators;
using UnityEngine;

namespace Code.BattleActions
{
    public class SimpleShotBattleAction : BattleAction, IDisposable
    {
        public Transform Target { get; }
        
        public float Damage { get; }

        public SimpleShotBattleAction(IBattleActionCreator creator, Transform target, float damage) : base(creator)
        {
            Target = target;
            Damage = damage;
        }

        protected override void PlayCore()
        {
            bool hit = Target.childCount > 0;
            
            SelfShipController.SimpleShot(Target, hit, Damage);

            if (hit)
            {
                SelfShipController.ShotFinished += OnShipControllerShotFinished;    
            }
        }

        private void OnShipControllerShotFinished(object sender, GunController.ShotFinishedEventArgs eventArgs)
        {
            EnemyShipController.TakeDamage(eventArgs.Shot.Damage);
        }

        public override void Dispose()
        {
            if (SelfShipController != null)
            {
                SelfShipController.ShotFinished -= OnShipControllerShotFinished;    
            }
        }
    }
}