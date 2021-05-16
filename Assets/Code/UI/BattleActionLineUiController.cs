using System;
using System.Linq;
using Code.Battle;
using Code.Battle.Actions;
using Code.Utils;
using UnityEngine;

namespace Code.UI
{
    public class BattleActionLineUiController : BattleUi
    {
        public event EventHandler BeginPlay;
        public event EventHandler EndPlay;
        public event EventHandler<EventArgs<PlayerSide>> PlayerReady;
        public event EventHandler<EventArgs<BattleAction>> CancelAction;
        
        public bool Finished => _currentPlayStep == null;

        protected override Canvas Canvas { get; set; }

        private BattleActionQueueController[] _actionQueueControllers;

        private BattleStep _currentPlayStep;

        private bool playing;
        
        public override void Enable()
        {
        }

        public override void Disable()
        {
        }

        public void AddBattleAction(BattleAction action)
        {
            foreach (BattleActionQueueController actionQueueController in _actionQueueControllers)
            {
                if (actionQueueController.PlayerSide == action.PlayerSide)
                {
                    actionQueueController.Enqueue(action);
                }
            }
        }

        public void Ready()
        {
            PlayerSide? managedPlayer = ReferenceItems.BattleSettings.ManagedPlayer;

            if (managedPlayer == null)
            {
                Play();
            }
            else
            {
                PlayerReady?.Invoke(this, new EventArgs<PlayerSide>((PlayerSide)managedPlayer));    
            }
        }
        
        public void Play()
        {
            if (playing)
            {
                return;
            }
            
            playing = true;
            BeginPlay?.Invoke(this, EventArgs.Empty);
            _currentPlayStep = PlayNextStep();
        }

        public void RevertAction()
        {
            if (ReferenceItems.BattleSettings.ManagedPlayer == null)
            {
                return;
            }
            
            BattleAction battleAction = _actionQueueControllers
                .First(x => x.PlayerSide == ReferenceItems.BattleSettings.ManagedPlayer)
                .Dequeue();

            if (battleAction == null)
            {
                return;
            }

            battleAction.Dispose();

            CancelAction?.Invoke(this, new EventArgs<BattleAction>(battleAction));
        }

        private void Awake()
        {
            _actionQueueControllers = GetComponentsInChildren<BattleActionQueueController>();
            Canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            if (!Finished)
            {
                _currentPlayStep.Update();
            }
        }

        private BattleStep PlayNextStep()
        {
            BattleAction[] actions = _actionQueueControllers.Select(x => x.Dequeue()).Where(x => x != null).ToArray();

            if (actions.Length == 0)
            {
                return null;
            }

            var battleStep = new BattleStep(actions, ReferenceItems.ShipControllers, ReferenceItems.BattleZoneDescription);
            battleStep.Finished += OnBattleStepFinished; 
            
            battleStep.Play();

            return battleStep;
        }

        private void OnBattleStepFinished(object sender, EventArgs eventArgs)
        {
            var battleStep = (BattleStep) sender;
            battleStep.Finished -= OnBattleStepFinished;
            
            _currentPlayStep.Dispose();
            _currentPlayStep = PlayNextStep();

            if (_currentPlayStep == null)
            {
                EndPlay?.Invoke(this, EventArgs.Empty);
                        
                playing = false;
            }
        }
    }
}