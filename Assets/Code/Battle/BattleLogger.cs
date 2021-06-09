using System;
using BattleCore;
using BattleCore.Actions;
using BattleCore.Log;
using UnityEngine;

namespace Code.Battle
{
    public class BattleLogger : IBattleLogger
    {
        [Flags]
        public enum LogGroup
        {
            None = 0,
            Common = 1,
            Result = 2,
            ShipParameters = 4,
            Actions = 8,
            Step = 16
        }

        private readonly LogGroup _showLogFlags;

        public BattleLogger(LogGroup showLogFlags)
        {
            _showLogFlags = showLogFlags;
        }

        public void LogMessage(BattleLoggerMessageType messageType, string message)
        {
            if (_showLogFlags.HasFlag(LogGroup.Common))
            {
                Debug.Log(message);
            }
        }

        public void LogShipParameters(IBattleShip ship, IBattleZone zone, string message)
        {
            if (_showLogFlags.HasFlag(LogGroup.ShipParameters))
            {
                // ReSharper disable once UseStringInterpolation
                string logMessage = string.Format(
                    "{0}|{1}({2})|{3}({4})|{5}({6})",
                    ship.PlayerSide,
                    nameof(zone),
                    zone.GetShipBattleZoneField(ship.PlayerSide)?.Index,
                    nameof(ship.Health),
                    ship.Health,
                    nameof(ship.Energy),
                    ship.Energy
                );
            
                Debug.Log(logMessage);
            }
        }

        public void LogAction(BattleAction battleAction, string message)
        {
            if (_showLogFlags.HasFlag(LogGroup.Actions))
            {
                Debug.Log($"{battleAction.PlayerSide}|{battleAction.Id}:{message}");
            }
        }

        public void LogStep(int stepIndex, string message)
        {
            if (_showLogFlags.HasFlag(LogGroup.Step))
            {
                Debug.Log($"Step {stepIndex}. {message}");
            }
        }

        public void LogWinner(PlayerSide playerSide, string message)
        {
            if (_showLogFlags.HasFlag(LogGroup.Result))
            {
                Debug.Log($"{playerSide} winner! {message}");    
            }
        }
    }
}