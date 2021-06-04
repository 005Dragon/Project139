using BattleCore.Actions;

namespace BattleCore.Log
{
    public interface IBattleLogger
    {
        void LogMessage(BattleLoggerMessageType messageType, string message);

        void LogPlayerMessage(BattleLoggerMessageType messageType, PlayerSide playerSide, string message);

        void LogShipMessage(BattleLoggerMessageType messageType, IBattleShip ship, string message);
        
        void LogActionMessage(BattleLoggerMessageType messageType, BattleAction battleAction, string message);
    }
}