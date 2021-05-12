using Code.BattleActions;
using UnityEngine;

namespace Code.BattleActionCreators
{
    public abstract class BattleActionCreatorBase : MonoBehaviour, IBattleActionCreator
    {
        public PlayerId Player { get; set; }

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