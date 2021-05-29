using UnityEngine;

namespace Code.Battle
{
    public class  ReferenceItemsController : MonoBehaviour
    {
        public IBattleReferenceItems ReferenceItems => _referenceItems;
        
        [SerializeField]
        private Camera _camera;
        
        [SerializeField]
        private BattleZoneDescription _battleZoneDescription;
        
        [SerializeField] 
        private ShipController[] _shipControllers;

        private BattleReferenceItems _referenceItems;

        private void Awake()
        {
            _referenceItems = new BattleReferenceItems
            {
                BattleSettings = GetComponent<BattleSettings>(),
                BattleZoneDescription = _battleZoneDescription,
                Camera = _camera,
                ShipControllers = _shipControllers
            };
        }
    }
}