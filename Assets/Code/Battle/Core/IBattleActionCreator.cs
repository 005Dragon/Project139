using Code.Battle.Core.Actions;
using Code.Battle.Core.Log;
using UnityEngine;

namespace Code.Battle.Core
{
    public interface IBattleActionCreator
    {
        public BattleActionId ActionId { get; }
        
        public BattleActionType ActionType { get; }
        
        public IBattleLogger Logger { get; set; }
        
        public PlayerSide PlayerSide { get; set; }
        
        public Sprite Sprite { get; set; }
        
        public float EnergyCost { get; }
        
        public BattleAction Create();
    }
}