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
            _battleEngine.PlayNextRound();
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

            IBattlePlayer[] players = CreatePlayers().ToArray();

            foreach (IBattlePlayer player in players)
            {
                player.ReadyChange += OnBattlePlayerReadyChange;
            }
            
            _battleEngine = new BattleEngine(
                players,
                Service.BattleShipsService.GetBattleShips().ToArray(),
                Service.BattleZone,
                _battleActionLineUiController.BottomBarControllers.Select(x => (IBattleActionQueue)x.BattleActionQueue).ToArray(),
                Service.BattleActionCreatorService.GetBattleActionCreators().ToArray(),
                battleLogger,
                new UnityRandom()
            );
            
            _battleEngine.Initialize();
        }

        private void OnBattlePlayerReadyChange(object sender, EventArgs<bool> eventArgs)
        {
            var player = (IBattlePlayer) sender;

            BattleBottomBarController battleBottomBarController = 
                _battleActionLineUiController.BottomBarControllers.First(x => x.BattleActionQueue.PlayerSide == player.PlayerSide);
            
            battleBottomBarController.SetReady(eventArgs.Value);
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