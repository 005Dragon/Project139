using System.Collections.Generic;
using System;
using System.Linq;
using BattleCore;
using BattleCore.UI;
using BattleCore.Utils;
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
        private GameObject _battleActionTemplate;
        
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
        
        public void AddEnableBattleActionCreators(PlayerSide playerSide, IEnumerable<IBattleActionCreator> battleActionCreators)
        {
            IBattleActionCreator[] battleActionCreatorsArray = battleActionCreators.ToArray();
            
            foreach (BarController barController in _barControllers)
            {
                if (barController.playerSide == playerSide)
                {
                    _activeBarController = barController;

                    _activeBarController.Build(_battleActionTemplate, battleActionCreatorsArray);
                    
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
        
        private void OnCreateBattleAction(object sender, EventArgs<IBattleActionCreator> eventArgs)
        {
            if (eventArgs.Value is ITargetBattleActionCreator targetBattleActionCreator)
            {
                _canvas.enabled = false;
                
                _battleSelectTargetUiController.SetTargetBattleActionCreator(targetBattleActionCreator);
            }
            else
            {
                CreateBattleAction?.Invoke(this, eventArgs);                
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