using System;
using UnityEngine;

namespace Code.UI
{
    public class BattleMainUiController : BattleUi
    {
        public event EventHandler<CreateBattleActionEventArgs> CreateBattleAction;

        private BarController[] _barControllers;

        protected override Canvas Canvas { get; set; }

        public override void Build()
        {
            foreach (BarController barController in _barControllers)
            {
                barController.Build(ReferenceItems.BattleSettings.BattleActionTemplates);    
            }
        }

        public void Reset()
        {
            foreach (BarController barController in _barControllers)
            {
                barController.ResetTabs();
            }
        }

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();

            _barControllers = GetComponentsInChildren<BarController>();

            foreach (BarController barController in _barControllers)
            {
                barController.CreateBattleAction += OnCreateBattleAction;
            }
        }

        private void OnCreateBattleAction(object sender, CreateBattleActionEventArgs eventArgs)
        {
            CreateBattleAction?.Invoke(this, eventArgs);
        }
    }
}