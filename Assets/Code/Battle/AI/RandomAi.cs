using System.Linq;
using Code.Battle.ActionCreators;
using Code.UI;
using Code.UI.BattleUiController;
using Code.Utils;
using UnityEngine;

namespace Code.Battle.AI
{
    public class RandomAi : MonoBehaviour
    {
        private IBattleReferenceItems ReferenceItems => _referenceItemsController.ReferenceItems;

        private ReferenceItemsController _referenceItemsController;

        private bool _disabled;

        private PlayerSide[] _aiPlayers;

        private BattleUiController _battleUiController;

        private IBattleActionCreator[] _battleActionCreators;
        
        private void Awake()
        {
            _referenceItemsController = GetComponent<ReferenceItemsController>();
            _battleUiController = GetComponent<BattleUiController>();
        }

        private void Start()
        {
            _aiPlayers = ReferenceItems.BattleSettings.PlayersSettings
                .Where(x => x.PlayerManagementType == PlayerManagementType.RandomAi)
                .Select(x => x.playerSide)
                .ToArray();

            _disabled = _aiPlayers.Length == 0;
            
            _battleActionCreators = ReferenceItems.BattleSettings.BattleActionTemplates
                .Select(x => x.GetComponentInChildren<BattleActionController>().BattleActionCreator).ToArray();
        }

        private void Update()
        {
            if (_disabled)
            {
                return;
            }

            foreach (PlayerSide player in _aiPlayers)
            {
                ShipController selfShipController = ReferenceItems.ShipControllers.First(x => x.PlayerSide == player);

                while (selfShipController.Energy > 0)
                {
                    GenerateNextBattleAction(player);
                }
                
                _battleUiController.Ready(player);
            }
        }

        private void GenerateNextBattleAction(PlayerSide player)
        {
            IBattleActionCreator actionCreator = _battleActionCreators[Random.Range(0, _battleActionCreators.Length)];

            actionCreator.PlayerSide = player;
            
            if (actionCreator is ITargetBattleActionCreator targetBattleActionCreator)
            {
                IBattleZoneField[] enableTargets =
                    targetBattleActionCreator.GetEnableTargets(ReferenceItems.BattleZoneDescription).ToArray();

                IBattleZoneField target = enableTargets[Random.Range(0, enableTargets.Length)];
                
                targetBattleActionCreator.SetTargets(target.ToSingleElementEnumerable());
            }

            _battleUiController.TryCreateBattleAction(actionCreator);
        }
    }
}