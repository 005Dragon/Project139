using BattleCore.Actions;

namespace BattleCore.Log
{
    public interface IBattleLogger
    {
        void LogMessage(BattleLoggerMessageType messageType, string message);

        void LogShipParameters(IBattleShip ship, IBattleZone zone, string message);

        void LogAction(BattleAction battleAction, string message);
        
        void LogStep(int stepIndex, string message);

        void LogWinner(PlayerSide playerSide, string message);
    }
}