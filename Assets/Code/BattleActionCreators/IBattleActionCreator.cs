using Code.BattleActions;
using UnityEngine;

namespace Code.BattleActionCreators
{
    public interface IBattleActionCreator
    {
        public PlayerId Player { get; set; }
        
        public BattleActionType ActionType { get; }
        
        public Sprite Sprite { get; set; }
        
        public float EnergyCost { get; }
        
        public BattleAction Create();
    }
}