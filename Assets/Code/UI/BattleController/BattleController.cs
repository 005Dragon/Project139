using System;
using System.Collections.Generic;
using System.Linq;
using Code.BattleActionCreators;
using Code.Utils;
using UnityEngine;

namespace Code.UI.BattleController
{
    public partial class BattleController : MonoBehaviour
    {
        private readonly List<BattleUi> _uis = new List<BattleUi>();

        private IBattleReferenceItems ReferenceItems => _referenceItemsController.ReferenceItems;

        private ReferenceItemsController _referenceItemsController;

        private readonly HashSet<PlayerId> _readyPlayers = new HashSet<PlayerId>();

        private BattleUi _activeUi;
        
        public bool TryCreateBattleAction(IBattleActionCreator creator)
        {
            if (!CanCreateBattleAction(creator))
            {
                return false;
            }
            
            CreateBattleAction(creator);

            return true;
        }

        public void Ready(PlayerId player)
        {
            if (_activeUi == _battleResultUiController)
            {
                return;
            }
            
            _readyPlayers.Add(player);

            int activePlayersCount =
                ReferenceItems.BattleSettings.PlayersSettings.Count(x => x.PlayerManagementType != PlayerManagementType.None);
            
            if (activePlayersCount == _readyPlayers.Count)
            {
                _battleActionLineUiController.Play();
                _readyPlayers.Clear();
            }
        }

        private void Start()
        {
            _referenceItemsController = GetComponent<ReferenceItemsController>();
            
            foreach (ShipController shipController in ReferenceItems.ShipControllers)
            {
                shipController.ShipDestroy += OnShipControllerShipDestroy;
            }
            
            InitializeMainUiController();
            InitializeSelectTargetUiController();
            InitializeActionLineUiController();
            InitializeResultUiController();
            InitializeStatsUiController();
            
            foreach (BattleUi battleUi in _uis)
            {
                battleUi.Build();
            }
            
            Reset();
        }

        private void SetActiveUi(BattleUi ui)
        {
            foreach (BattleUi battleUi in _uis)
            {
                if (battleUi == ui)
                {
                    battleUi.Enable();
                    _activeUi = battleUi;
                }
                else
                {
                    battleUi.Disable();
                }
            }
        }
        
        private void Reset()
        {
            SetActiveUi(_uis[0]);
        }

        private void OnShipControllerShipDestroy(object sender, EventArgs eventArgs)
        {
            SetActiveUi(_battleResultUiController);
        }
    }
}