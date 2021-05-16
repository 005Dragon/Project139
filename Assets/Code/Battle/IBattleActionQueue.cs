using Code.Battle.Actions;

namespace Code.Battle
{
    public interface IBattleActionQueue
    {
        PlayerSide PlayerSide { get; }

        public void Enqueue(BattleAction action);
        
        public BattleAction Dequeue();
    }
}