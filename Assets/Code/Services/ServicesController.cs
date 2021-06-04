using Code.Battle;
using Code.Services.Controllers;
using UnityEngine;

namespace Code.Services
{
    public class ServicesController : MonoBehaviour
    {
        [SerializeField] 
        private BattleSettingsServiceController _battleSettingsServiceController;

        [SerializeField]
        private BattleShipsServiceController _battleShipsServiceController;
        
        [SerializeField] 
        private BattleActionImageServiceController _battleActionImageServiceController;

        [SerializeField] 
        private BattleActionCreatorServiceController _battleActionCreatorServiceController;

        [SerializeField] 
        private BattleZoneController _battleZoneController;
        
        private void Awake()
        {
            Service.Initialize(
                InitializeIfNull(_battleSettingsServiceController),
                InitializeIfNull(_battleShipsServiceController),
                InitializeIfNull(_battleActionImageServiceController),
                InitializeIfNull(_battleActionCreatorServiceController),
                InitializeIfNull(_battleZoneController)
            );
        }

        private T InitializeIfNull<T>(T value)
            where T : MonoBehaviour
        {
            return value == null ? GetComponentInChildren<T>() : value;
        }
    }
}