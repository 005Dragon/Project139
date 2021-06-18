using BattleCore;
using BattleCore.Actions;
using BattleCore.Log;
using UnityEngine;

namespace Code.Battle.ActionCreators
{
    public abstract class BattleActionCreatorBase : MonoBehaviour, IBattleActionCreator
    {
        public abstract BattleActionId ActionId { get; }
        
        public abstract BattleActionType ActionType { get; }
        
        public IBattleLogger Logger { get; set; }
        
        public float EnergyCost => _energyCost;

        public float Duration => _duration;
        
        [SerializeField]
        protected float _energyCost;

        [SerializeField]
        protected float _duration;

        public abstract BattleAction Create(PlayerSide playerSide);
        public abstract object Clone();
    }
}