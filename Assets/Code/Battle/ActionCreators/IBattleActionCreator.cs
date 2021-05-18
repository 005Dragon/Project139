using Code.Battle.Actions;
using Code.Battle.Log;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public interface IBattleActionCreator
    {
        public IBattleLogger Logger { get; set; }
        
        public PlayerSide PlayerSide { get; set; }
        
        public BattleActionType ActionType { get; }
        
        public Sprite Sprite { get; set; }
        
        public float EnergyCost { get; }
        
        public BattleAction Create();
    }
}