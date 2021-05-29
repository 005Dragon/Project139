using System.Collections.Generic;
using Code.Battle.Core;
using Code.UI;
using System;
using System.Linq;
using Code.Battle.Core.UI;
using Code.Utils;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleActionBarController : MonoBehaviour, IUiBattleActionBar
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;

        public bool Visible
        {
            get => _canvas.enabled;
            set => _canvas.enabled = value;
        }

        [SerializeField]
        private GameObject[] _battleActionTemplates;

        private BarController[] _barControllers;
        private BarController _activeBarController;

        private Canvas _canvas;

        private BattleSelectTargetUiController _battleSelectTargetUiController;

        public void Initialize(BattleSelectTargetUiController battleSelectTargetUiController)
        {
            _battleSelectTargetUiController = battleSelectTargetUiController;

            _battleSelectTargetUiController.Apply += OnBattleSelectTargetApply;
            _battleSelectTargetUiController.Cancel += OnBattleSelectTargetCancel;
        } 

        public IEnumerable<IBattleActionCreator> GetAllBattleActionCreators()
        {
            return _battleActionTemplates.Select(x => x.GetComponent<BattleActionController>().BattleActionCreator);
        }
        
        public void AddEnableBattleActionCreators(PlayerSide playerSide, IEnumerable<IBattleActionCreator> battleActionCreators)
        {
            IBattleActionCreator[] battleActionCreatorsArray = battleActionCreators.ToArray();
            
            foreach (BarController barController in _barControllers)
            {
                if (barController.playerSide == playerSide)
                {
                    _activeBarController = barController;
                    
                    _activeBarController.Build(
                        GetEnableBattleActionTemplates(battleActionCreatorsArray),
                        battleActionCreatorsArray.First().Logger
                    );
                    
                    _activeBarController.CreateBattleAction += OnCreateBattleAction;
                }
                else
                {
                    Destroy(barController.gameObject);
                }
            }
        }

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            _barControllers = GetComponentsInChildren<BarController>();
        }
        
        private void OnCreateBattleAction(object sender, CreateBattleActionEventArgs eventArgs)
        {
            if (eventArgs.BattleActionCreator is ITargetBattleActionCreator targetBattleActionCreator)
            {
                _canvas.enabled = false;
                
                _battleSelectTargetUiController.SetTargetBattleActionCreator(targetBattleActionCreator);
            }
            else
            {
                CreateBattleAction?.Invoke(this, new EventArgs<IBattleActionCreator>(eventArgs.BattleActionCreator));                
            }
        }
        
        private IEnumerable<GameObject> GetEnableBattleActionTemplates(IEnumerable<IBattleActionCreator> enableBattleActionCreators)
        {
            Dictionary<BattleActionController, GameObject> controllerToTemplateIndex = new Dictionary<BattleActionController, GameObject>();

            foreach (GameObject template in _battleActionTemplates)
            {
                controllerToTemplateIndex.Add(template.GetComponent<BattleActionController>(), template);
            }

            foreach (IBattleActionCreator enableBattleActionCreator in enableBattleActionCreators)
            {
                if (controllerToTemplateIndex.Keys.All(x => x.BattleActionCreator.ActionId != enableBattleActionCreator.ActionId))
                {
                    continue;
                }
                
                KeyValuePair<BattleActionController, GameObject> controllerTemplatePair =
                    controllerToTemplateIndex.First(x => x.Key.BattleActionCreator.ActionId == enableBattleActionCreator.ActionId);

                yield return controllerTemplatePair.Value;
            }
        }
        
        private void OnBattleSelectTargetCancel(object sender, EventArgs eventArgs)
        {
            _canvas.enabled = true;
        }

        private void OnBattleSelectTargetApply(object sender, EventArgs<ITargetBattleActionCreator> eventArgs)
        {
            _canvas.enabled = true;
            
            CreateBattleAction?.Invoke(this, new EventArgs<IBattleActionCreator>(eventArgs.Value));
        }
    }
}