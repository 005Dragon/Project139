using System;
using System.Collections.Generic;
using System.Linq;
using Code.Battle.Core;
using Code.UI;
using Code.UI.BattleUiController;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleUiController : MonoBehaviour
    {
        private IBattleReferenceItems ReferenceItems => _referenceItemsController.ReferenceItems;
        
        private BattleEngine _battleEngine;
        
        private ReferenceItemsController _referenceItemsController;

        private BattleActionLineUiController _battleActionLineUiController;
        private BattleActionBarController _battleActionBarController;
        private BattleSelectTargetUiController _battleSelectTargetUiController;
        private BattlePlayerReadyController _playerReadyController;
        private BattleStatsUiController _battleStatsUiController;

        private void Start()
        {
            Initialize();
            
            Build();
            
            _battleEngine.Play();
        }

        private void Initialize()
        {
            _referenceItemsController = GetComponent<ReferenceItemsController>();
            
            _battleActionLineUiController = GetComponentInChildren<BattleActionLineUiController>();
            _battleSelectTargetUiController = GetComponentInChildren<BattleSelectTargetUiController>();
            
            _battleActionBarController = GetComponentInChildren<BattleActionBarController>();
            _battleActionBarController.Initialize(_battleSelectTargetUiController);
            
            _playerReadyController = GetComponentInChildren<BattlePlayerReadyController>();
            
            _battleStatsUiController = GetComponentInChildren<BattleStatsUiController>();

            foreach (ShipController ship in ReferenceItems.ShipControllers)
            {
                ship.HealthChange += OnShipHealthChange;
                ship.EnergyChange += OnShipEnergyChange;
            }
            
            _battleEngine = new BattleEngine(
                CreatePlayers().ToArray(),
                ReferenceItems.ShipControllers.Select(x => (IBattleShip) x).ToArray(),
                ReferenceItems.BattleZoneDescription,
                _battleActionLineUiController.BattleActionQueues,
                _battleActionBarController.GetAllBattleActionCreators().ToArray(),
                new BattleLogger()
            );
            
            _battleEngine.Initialize();
        }

        private void Build()
        {
            if (ReferenceItems.BattleSettings.ManagedPlayer != null)
            {
                _battleSelectTargetUiController.Build(
                    (PlayerSide) ReferenceItems.BattleSettings.ManagedPlayer,
                    ReferenceItems.BattleZoneDescription
                );
            }

            _battleStatsUiController.Build(ReferenceItems.ShipControllers.Select(x => (IBattleShip) x).ToArray());
        }

        private IEnumerable<IBattlePlayer> CreatePlayers()
        {
            var battlePlayerCreator = new BattlePlayerCreator(
                new UnityRandom(),
                ReferenceItems.BattleZoneDescription,
                _battleActionBarController,
                _playerReadyController,
                ReferenceItems.ShipControllers.Select(x => (IBattleShip)x).ToArray()
            );

            foreach (BattleSettings.PlayerSettings playerSettings in ReferenceItems.BattleSettings.PlayersSettings)
            {
                yield return battlePlayerCreator.Create(playerSettings.playerSide, playerSettings.PlayerManagementType);
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