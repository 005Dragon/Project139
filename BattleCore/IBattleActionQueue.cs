using BattleCore.Actions;

namespace BattleCore
{
    public interface IBattleActionQueue
    {
        PlayerSide PlayerSide { get; }

        void Enqueue(BattleAction action);
        
        BattleAction Dequeue();
    }
}