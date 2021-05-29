using System;
using System.Collections.Generic;
using System.Linq;
using Code.Battle;
using Code.Battle.ActionCreators;
using Code.Battle.Core;
using Code.Utils;
using UnityEngine;

namespace Code.UI
{
    public class BattleSelectTargetUiController : BattleUi
    {
        public class ApplyEventArgs : EventArgs
        {
            public ITargetBattleActionCreator TargetBattleActionCreator { get; }

            public ApplyEventArgs(ITargetBattleActionCreator targetBattleActionCreator)
            {
                TargetBattleActionCreator = targetBattleActionCreator;
            }
        }
        
        public event EventHandler Cancel;
        public event EventHandler<ApplyEventArgs> Apply;

        protected override Canvas Canvas { get; set; }
        
        [SerializeField]
        private GameObject _selfTargetGlowTemplate;
        [SerializeField]
        private GameObject _enemyTargetGlowTemplate;
        
        private ITargetBattleActionCreator _targetBattleActionCreator;
        
        private readonly List<TargetGlowController> _targetGlowControllers = new List<TargetGlowController>();

        public void SetTargetBattleActionCreator(ITargetBattleActionCreator targetBattleActionCreator)
        {
            _targetBattleActionCreator = targetBattleActionCreator;
        }
        
        public void OnCancel()
        {
            Cancel?.Invoke(this, EventArgs.Empty);
        }

        public override void Enable()
        {
            base.Enable();
            
            UpdateVisibleGlows();
        }

        public override void Build()
        {
            Canvas.enabled = true;
            CreateGlows();
            Canvas.enabled = false;
        }
        
        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        private void CreateGlows()
        {
            if (ReferenceItems.BattleSettings.ManagedPlayer == null)
            {
                return;
            }

            IEnumerable<BattleZoneField> battleZoneFields =
                ReferenceItems.BattleZoneDescription.GetBattleZoneFields().Select(x => (BattleZoneField) x);
            
            foreach (BattleZoneField battleZoneField in battleZoneFields)
            {
                Vector2 position = battleZoneField.Transform.position;
                
                GameObject glowGameObject = Instantiate(
                    battleZoneField.PlayerSide == ReferenceItems.BattleSettings.ManagedPlayer
                        ? _selfTargetGlowTemplate
                        : _enemyTargetGlowTemplate, 
                    position,
                    Quaternion.identity,
                    transform
                );
                
                TargetGlowController glowController = glowGameObject.GetComponent<TargetGlowController>();
                glowController.BattleZoneField = battleZoneField;
                glowController.Selected += OnApply;
                glowController.Initialize();
                
                _targetGlowControllers.Add(glowController);
            }
        }

        private void UpdateVisibleGlows()
        {
            List<IBattleZoneField> enableTargets =
                _targetBattleActionCreator.GetEnableTargets(ReferenceItems.BattleZoneDescription).ToList();

            foreach (TargetGlowController targetGlowController in _targetGlowControllers)
            {
                BattleZoneField battleZoneField = targetGlowController.BattleZoneField;

                if (enableTargets.Exists(battleZoneField.Equals))
                {
                    targetGlowController.Show();
                }
                else
                {
                    targetGlowController.Hide();
                }
            }
        }
        
        private void OnApply(object sender, EventArgs args)
        {
            BattleZoneField battleZoneField = ((TargetGlowController) sender).BattleZoneField;
            
            _targetBattleActionCreator.SetTargets(battleZoneField.ToSingleElementEnumerable());

            Apply?.Invoke(this, new ApplyEventArgs(_targetBattleActionCreator));
        }
    }
}
