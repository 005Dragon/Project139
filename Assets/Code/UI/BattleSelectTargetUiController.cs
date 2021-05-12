using System;
using System.Collections.Generic;
using System.Linq;
using Code.BattleActionCreators;
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
            
            IEnumerable<PlayerBattleZoneDescription> playerBattleZoneDescriptions =
                ReferenceItems.BattleZoneDescription.GetPlayerBattleZoneDescriptions();
            
            foreach (PlayerBattleZoneDescription playerBattleZoneDescription in playerBattleZoneDescriptions)
            {
                Vector2 position = playerBattleZoneDescription.BattleZone.position;
                
                GameObject glowGameObject = Instantiate(
                    playerBattleZoneDescription.PlayerId == ReferenceItems.BattleSettings.ManagedPlayer
                        ? _selfTargetGlowTemplate
                        : _enemyTargetGlowTemplate, 
                    position,
                    Quaternion.identity,
                    transform
                );
                
                TargetGlowController glowController = glowGameObject.GetComponent<TargetGlowController>();
                glowController.PlayerBattleZoneDescription = playerBattleZoneDescription;
                glowController.Selected += OnApply;
                glowController.Initialize();
                
                _targetGlowControllers.Add(glowController);
            }
        }

        private void UpdateVisibleGlows()
        {
            List<Transform> enableTargets = _targetBattleActionCreator.GetEnableTargets(ReferenceItems.BattleZoneDescription).ToList();

            foreach (TargetGlowController targetGlowController in _targetGlowControllers)
            {
                Transform battleZone = targetGlowController.PlayerBattleZoneDescription.BattleZone;

                if (enableTargets.Exists(x => x == battleZone))
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
            PlayerBattleZoneDescription playerBattleZoneDescription = ((TargetGlowController) sender).PlayerBattleZoneDescription;
            
            _targetBattleActionCreator.SetTargets(playerBattleZoneDescription.BattleZone.ToSingleElementEnumerable());

            Apply?.Invoke(this, new ApplyEventArgs(_targetBattleActionCreator));
        }
    }
}
