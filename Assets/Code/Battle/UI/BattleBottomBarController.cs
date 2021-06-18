using System;
using BattleCore;
using BattleCore.Actions;
using BattleCore.Utils;
using Code.Services;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleBottomBarController : MonoBehaviour
    {
        public BattleActionQueueController BattleActionQueue { get; private set; }

        [SerializeField]
        private GameObject _foldingScreenGameObject;
        
        private LampController _greenLampController;
        private LampController _redLampController;

        public void SetReady(bool value)
        {
            if (value)
            {
                _greenLampController.On();
                _redLampController.Off();
            }
            else
            {
                _greenLampController.Off();
                _redLampController.On();
            }
        }

        private void Awake()
        {
            BattleActionQueue = GetComponentInChildren<BattleActionQueueController>();

            BattleActionQueue.ActivateAction += OnBattleActionQueueActivateAction;
            
            var lampControllers = GetComponentsInChildren<LampController>();

            float maxGreen = float.MinValue;
            float maxRed = float.MinValue;

            foreach (LampController lampController in lampControllers)
            {
                if (lampController.Color.g > maxGreen)
                {
                    maxGreen = lampController.Color.g;
                    _greenLampController = lampController;
                }

                if (lampController.Color.r > maxRed)
                {
                    maxRed = lampController.Color.r;
                    _redLampController = lampController;
                }
            }
        }

        private void Start()
        {
            PlayerManagementType playerManagementType = Service.BattleSettingsService.GetPlayerManagementType(BattleActionQueue.PlayerSide);

            if (playerManagementType == PlayerManagementType.Manual)
            {
                Destroy(_foldingScreenGameObject);
            }
            else
            {
                PlayerManagementType enemyPlayerManagementType =
                    Service.BattleSettingsService.GetPlayerManagementType(BattleActionQueue.PlayerSide.GetAnother());
            
                if (enemyPlayerManagementType != PlayerManagementType.Manual)
                {
                    Destroy(_foldingScreenGameObject);
                }
            }
        }

        private void OnBattleActionQueueActivateAction(object sender, EventArgs<BattleAction> eventArgs)
        {
            _greenLampController.Blink();
        }
    }
}