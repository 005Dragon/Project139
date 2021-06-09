using System;
using BattleCore.Log;

namespace BattleCore.Actions
{
    public class MoveDownAction : BattleAction
    {
        private IBattleZoneField _lastBattleZoneField;
        
        public MoveDownAction(PlayerSide playerSide, IBattleActionCreator creator) : base(playerSide, creator)
        {
        }

        protected override void PlayCore()
        {
            SelfShip.ChangeBattleZoneFinished += OnSelfShipChangeBattleZoneFinished;
            
            _lastBattleZoneField = BattleZone.GetShipBattleZoneField(PlayerSide);
            
            SelfShip.TryChangeBattleZone(Direction4.Down, out _);
        }

        private void OnSelfShipChangeBattleZoneFinished(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;

            ship.ChangeBattleZoneFinished -= OnSelfShipChangeBattleZoneFinished;
            
            IBattleZoneField currentBattleZoneField = BattleZone.GetShipBattleZoneField(PlayerSide);
            
            if (currentBattleZoneField.Equals(_lastBattleZoneField))
            {
                Logger.LogMessage(BattleLoggerMessageType.Warning, $"Failed to change the battle zone.");
            }
            
            Logger.LogAction(this, $"battle zone index - {currentBattleZoneField.Index}");
            
            Finish();
        }
    }
}