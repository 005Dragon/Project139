using System;
using BattleCore.Log;

namespace BattleCore.Actions
{
    public class MoveUpAction : BattleAction
    {
        private IBattleZoneField _currentBattleZoneField;
        private bool _battleZoneChanged;
        
        public MoveUpAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
            SelfShip.ChangeBattleZoneFinished += OnSelfShipChangeBattleZoneFinished;
            
            _battleZoneChanged = SelfShip.TryChangeBattleZone(Direction4.Up, out IBattleZoneField resultBattleZoneField);
            
            _currentBattleZoneField = _battleZoneChanged ? resultBattleZoneField : BattleZone.GetShipBattleZoneField(PlayerSide);
        }

        private void OnSelfShipChangeBattleZoneFinished(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;

            ship.ChangeBattleZoneFinished -= OnSelfShipChangeBattleZoneFinished;
            
            if (_battleZoneChanged)
            {
                Logger.LogActionMessage(BattleLoggerMessageType.Info, this, $"New battle zone index - {_currentBattleZoneField.Index}.");
            }
            else
            {
                Logger.LogActionMessage(
                    BattleLoggerMessageType.Warning,
                    this,
                    $"Failed to change the battle zone, current battle zone index - {_currentBattleZoneField.Index}."
                );
            }
            
            Finish();
        }
    }
}