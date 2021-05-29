using Code.Battle.Core;
using Code.Battle.Core.Actions;
using Code.Battle.Core.Log;
using UnityEngine;

namespace Code.Battle
{
    public class BattleLogger : IBattleLogger
    {
        public void LogMessage(BattleLoggerMessageType messageType, string message)
        {
            Debug.Log(message);
        }

        public void LogPlayerMessage(BattleLoggerMessageType messageType, PlayerSide playerSide, string message)
        {
            Debug.Log($"{playerSide}: {message}");
        }

        public void LogShipMessage(BattleLoggerMessageType messageType, IBattleShip ship, string message)
        {
            Debug.Log($"{ship.PlayerSide}|{nameof(ship.Health)}({ship.Health})|{nameof(ship.Energy)}({ship.Energy}):{message}");
        }

        public void LogActionMessage(BattleLoggerMessageType messageType, BattleAction battleAction, string message)
        {
            Debug.Log($"{battleAction.PlayerSide}|{battleAction.Id}:{message}");
        }
    }
}