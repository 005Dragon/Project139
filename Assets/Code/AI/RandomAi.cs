using System.Linq;
using Code.BattleActionCreators;
using Code.UI;
using Code.UI.BattleController;
using Code.Utils;
using UnityEngine;

namespace Code.AI
{
    public class RandomAi : MonoBehaviour
    {
        private IBattleReferenceItems ReferenceItems => _referenceItemsController.ReferenceItems;

        private ReferenceItemsController _referenceItemsController;

        private bool _disabled;

        private PlayerId[] _aiPlayers;

        private BattleController _battleController;

        private IBattleActionCreator[] _battleActionCreators;
        
        private void Awake()
        {
            _referenceItemsController = GetComponent<ReferenceItemsController>();
            _battleController = GetComponent<BattleController>();
        }

        private void Start()
        {
            _aiPlayers = ReferenceItems.BattleSettings.PlayersSettings
                .Where(x => x.PlayerManagementType == PlayerManagementType.RandomAi)
                .Select(x => x.PlayerId)
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

            foreach (PlayerId player in _aiPlayers)
            {
                ShipController selfShipController = ReferenceItems.ShipControllers.First(x => x.Player == player);

                while (selfShipController.Energy > 0)
                {
                    GenerateNextBattleAction(player);
                }
                
                _battleController.Ready(player);
            }
        }

        private void GenerateNextBattleAction(PlayerId player)
        {
            IBattleActionCreator actionCreator = _battleActionCreators[Random.Range(0, _battleActionCreators.Length)];

            actionCreator.Player = player;
            
            if (actionCreator is ITargetBattleActionCreator targetBattleActionCreator)
            {
                Transform[] enableTargets = targetBattleActionCreator.GetEnableTargets(ReferenceItems.BattleZoneDescription).ToArray();

                Transform target = enableTargets[Random.Range(0, enableTargets.Length)];
                
                targetBattleActionCreator.SetTargets(target.ToSingleElementEnumerable());
            }

            _battleController.TryCreateBattleAction(actionCreator);
        }
    }
}