using Code.BattleActionCreators;
using UnityEngine;

namespace Code.BattleActions
{
    public class DirectShotBattleAction : BattleAction
    {
        public float Damage { get; }
        
        public DirectShotBattleAction(IBattleActionCreator creator, float damage) : base(creator)
        {
            Damage = damage;
        }

        protected override void PlayCore()
        {
            Transform selfShipBattleZone = BattleZoneDescription.GetShipBattleZone(Player);

            BattleZoneDescription.TryGetBattleZoneByDirection(
                selfShipBattleZone,
                Player == PlayerId.Left ? Direction4.Right : Direction4.Left,
                out Transform target
            );

            bool hit = target.childCount > 0;
            
            SelfShipController.DirectionShot(target, hit, Damage);
            
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