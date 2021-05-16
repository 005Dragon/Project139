using Code.Battle.ActionCreators;
using Code.Utils;

namespace Code.Battle.Actions
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
            BattleZone.TryGetRelativeBattleZoneFieldByDirection(
                PlayerSide,
                PlayerSide == PlayerSide.Left ? Direction4.Right : Direction4.Left,
                out IBattleZoneField target
            );

            SelfShip.DirectionShot(target, Damage);

            SelfShip.ShotFinished += OnShipControllerShotFinished;
        }

        private void OnShipControllerShotFinished(object sender, EventArgs<ShotModel> eventArgs)
        {
            if (eventArgs.Value.Target.TryGetShip(out IBattleShip ship))
            {
                ship.TakeDamage(eventArgs.Value.Damage);
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