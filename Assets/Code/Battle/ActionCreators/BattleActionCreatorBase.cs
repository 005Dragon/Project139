using Code.Battle.Actions;
using Code.Battle.Log;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public abstract class BattleActionCreatorBase : MonoBehaviour, IBattleActionCreator
    {
        public IBattleLogger Logger { get; set; }
        
        public PlayerSide PlayerSide { get; set; }

        public BattleActionType ActionType => _actionType;
        
        public Sprite Sprite { get; set; }

        public float EnergyCost => _energyCost;

        [SerializeField]
        private BattleActionType _actionType; 
        
        [SerializeField]
        private float _energyCost;

        public abstract BattleAction Create();
    }
}