using BattleCore.Actions;

namespace BattleCore
{
    public interface IBattleActionQueue
    {
        bool IsEmpty { get; }
        
        PlayerSide PlayerSide { get; }

        void Enqueue(BattleAction action);
        
        BattleAction Dequeue();

        void Clear();
    }
}