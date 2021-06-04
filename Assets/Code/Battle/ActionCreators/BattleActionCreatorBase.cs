using Code.Battle.Core;
using Code.Battle.Core.Actions;
using Code.Battle.Core.Log;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public abstract class BattleActionCreatorBase : MonoBehaviour, IBattleActionCreator
    {
        public abstract BattleActionId ActionId { get; }
        
        public IBattleLogger Logger { get; set; }
        
        public BattleActionType ActionType => _actionType;
        
        public float EnergyCost => _energyCost;

        [SerializeField]
        private BattleActionType _actionType; 
        
        [SerializeField]
        private float _energyCost;

        public abstract BattleAction Create(PlayerSide playerSide);
    }
}