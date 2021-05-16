using Code.Battle.Actions;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public interface IBattleActionCreator
    {
        public PlayerSide PlayerSide { get; set; }
        
        public BattleActionType ActionType { get; }
        
        public Sprite Sprite { get; set; }
        
        public float EnergyCost { get; }
        
        public BattleAction Create();
    }
}