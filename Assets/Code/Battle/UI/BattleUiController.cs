using System;
using System.Collections.Generic;
using System.Linq;
using BattleCore;
using BattleCore.Log;
using BattleCore.Utils;
using Code.Services;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleUiController : MonoBehaviour
    {
        private BattleEngine _battleEngine;
        
        private BattleActionLineUiController _battleActionLineUiController;
        private BattleActionBarController _battleActionBarController;
        private BattleSelectTargetUiController _battleSelectTargetUiController;
        private BattlePlayerReadyController _playerReadyController;
        private BattleStatsUiController _battleStatsUiController;

        private void Start()
        {
            Initialize();
            
            Build();
        }

        private void Update()
        {
            _battleEngine.Play();
        }

        private void Initialize()
        {
            _battleActionLineUiController = GetComponentInChildren<BattleActionLineUiController>();
            _battleSelectTargetUiController = GetComponentInChildren<BattleSelectTargetUiController>();
            
            _battleActionBarController = GetComponentInChildren<BattleActionBarController>();
            _battleActionBarController.Initialize(_battleSelectTargetUiController);
            
            _playerReadyController = GetComponentInChildren<BattlePlayerReadyController>();
            
            _battleStatsUiController = GetComponentInChildren<BattleStatsUiController>();

            foreach (IBattleShip ship in Service.BattleShipsService.GetBattleShips())
            {
                ship.HealthChange += OnShipHealthChange;
                ship.EnergyChange += OnShipEnergyChange;
            }

            IBattleLogger battleLogger = new BattleLogger(
                BattleLogger.LogGroup.Common |
                BattleLogger.LogGroup.Result |
                //BattleLogger.LogGroup.ShipParameters |
                BattleLogger.LogGroup.Actions |
                BattleLogger.LogGroup.Step
            );
            
            _battleEngine = new BattleEngine(
                CreatePlayers().ToArray(),
                Service.BattleShipsService.GetBattleShips().ToArray(),
                Service.BattleZone,
                _battleActionLineUiController.BattleActionQueues,
                Service.BattleActionCreatorService.GetBattleActionCreators().ToArray(),
                battleLogger
            );
            
            _battleEngine.Initialize();
        }

        private void Build()
        {
            if (Service.BattleSettingsService.TryGetManagedPlayer(out PlayerSide managedPlayerSide))
            {
                _battleSelectTargetUiController.Build(managedPlayerSide, Service.BattleZone);
            }

            _battleStatsUiController.Build(Service.BattleShipsService.GetBattleShips().ToArray());
        }

        private IEnumerable<IBattlePlayer> CreatePlayers()
        {
            var battlePlayerCreator = new BattlePlayerCreator(
                new UnityRandom(),
                Service.BattleZone,
                _battleActionBarController,
                _playerReadyController,
                Service.BattleShipsService.GetBattleShips().ToArray()
            );

            foreach (PlayerSide playerSide in EnumExtensions.GetAllValues<PlayerSide>())
            {
                yield return battlePlayerCreator.Create(playerSide, Service.BattleSettingsService.GetPlayerManagementType(playerSide));
            }
        }
        
        private void OnShipHealthChange(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;
            
            _battleStatsUiController.SetHealth(ship.PlayerSide, ship.Health);
        }
        
        private void OnShipEnergyChange(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;
            
            _battleStatsUiController.SetEnergy(ship.PlayerSide, ship.Energy);
        }
    }
}