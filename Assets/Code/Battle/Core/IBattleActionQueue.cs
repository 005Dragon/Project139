using Code.Battle.Core.Actions;

namespace Code.Battle.Core
{
    public interface IBattleActionQueue
    {
        PlayerSide PlayerSide { get; }

        public void Enqueue(BattleAction action);
        
        public BattleAction Dequeue();
    }
}