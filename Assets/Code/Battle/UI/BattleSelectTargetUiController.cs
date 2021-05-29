using System;
using System.Collections.Generic;
using System.Linq;
using Code.Battle.Core;
using Code.UI;
using Code.Utils;
using UnityEngine;

namespace Code.Battle.UI
{
    public class BattleSelectTargetUiController : MonoBehaviour
    {
        public event EventHandler Cancel;
        public event EventHandler<EventArgs<ITargetBattleActionCreator>> Apply;

        private Canvas _canvas;
        
        [SerializeField]
        private GameObject _selfTargetGlowTemplate;
        [SerializeField]
        private GameObject _enemyTargetGlowTemplate;

        private IBattleZone _battleZone;
        private ITargetBattleActionCreator _targetBattleActionCreator;
        
        private readonly List<TargetGlowController> _targetGlowControllers = new List<TargetGlowController>();

        public void SetTargetBattleActionCreator(ITargetBattleActionCreator targetBattleActionCreator)
        {
            _targetBattleActionCreator = targetBattleActionCreator;

            _canvas.enabled = true;
            
            UpdateVisibleGlows(_battleZone);
        }
        
        public void OnCancel()
        {
            _canvas.enabled = false;
            
            Cancel?.Invoke(this, EventArgs.Empty);
        }

        public void Build(PlayerSide managedPlayerSide, IBattleZone battleZone)
        {
            _battleZone = battleZone;
            
            _canvas.enabled = true;
            CreateGlows(managedPlayerSide, battleZone);
            _canvas.enabled = false;
        }
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void CreateGlows(PlayerSide managedPlayerSide, IBattleZone battleZone)
        {
            IEnumerable<BattleZoneField> battleZoneFields = battleZone.GetBattleZoneFields().Select(x => (BattleZoneField) x);
            
            foreach (BattleZoneField battleZoneField in battleZoneFields)
            {
                Vector2 position = battleZoneField.Transform.position;
                
                GameObject glowGameObject = Instantiate(
                    battleZoneField.PlayerSide == managedPlayerSide ? _selfTargetGlowTemplate : _enemyTargetGlowTemplate, 
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

        private void UpdateVisibleGlows(IBattleZone battleZone)
        {
            List<IBattleZoneField> enableTargets = _targetBattleActionCreator.GetEnableTargets(battleZone).ToList();

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
            _canvas.enabled = false;
            
            BattleZoneField battleZoneField = ((TargetGlowController) sender).BattleZoneField;
            
            _targetBattleActionCreator.SetTargets(battleZoneField.ToSingleElementEnumerable());

            Apply?.Invoke(this, new EventArgs<ITargetBattleActionCreator>(_targetBattleActionCreator));
        }
    }
}
