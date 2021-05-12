using System;
using System.Collections.Generic;
using System.Linq;
using Code.BattleActions;
using Code.Utils;
using UnityEngine;

namespace Code.UI
{
    public class BattleActionLineUiController : BattleUi
    {
        private class PlayStep : IDisposable
        {
            public bool Finished => _actions.All(x => x.Finished);

            private readonly BattleAction[] _actions;
            private readonly List<BattleAction[]> _battleActionBatches = new List<BattleAction[]>();

            private bool _play;
            private int _internalStep = -1;
            
            private ShipController[] _shipControllers;
            private BattleZoneDescription _battleZoneDescription;
            
            public PlayStep(BattleAction[] actions)
            {
                _actions = actions;
                
                _battleActionBatches.Add(_actions.Where(x => x.ActionType == BattleActionType.Internal).ToArray());
                _battleActionBatches.Add(_actions.Where(x => x.ActionType == BattleActionType.Defense).ToArray());
                _battleActionBatches.Add(_actions.Where(x => x.ActionType == BattleActionType.Attack).ToArray());
            }

            public void Update()
            {
                if (_play)
                {
                    if (_internalStep == -1 || _battleActionBatches[_internalStep].All(x => x.Finished))
                    {
                        _internalStep++;

                        if (_internalStep == _battleActionBatches.Count)
                        {
                            _play = false;

                            return;
                        }

                        foreach (BattleAction battleAction in _battleActionBatches[_internalStep])
                        {
                            battleAction.Play(_shipControllers, _battleZoneDescription);
                        }
                    }
                }
            }

            public void Play(ShipController[] shipControllers, BattleZoneDescription battleZoneDescription)
            {
                _shipControllers = shipControllers;
                _battleZoneDescription = battleZoneDescription;
                
                _play = true;
            }

            public void Dispose()
            {
                foreach (BattleAction action in _actions)
                {
                    action?.Dispose();
                }
            }
        }

        public event EventHandler BeginPlay;
        public event EventHandler EndPlay;
        public event EventHandler<EventArgs<PlayerId>> PlayerReady;
        public event EventHandler<EventArgs<BattleAction>> CancelAction;
        
        public bool Finished => _currentPlayStep == null;

        protected override Canvas Canvas { get; set; }

        private BattleActionQueueController[] _actionQueueControllers;

        private PlayStep _currentPlayStep;

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
                if (actionQueueController.Player == action.Player)
                {
                    actionQueueController.Enqueue(action);
                }
            }
        }

        public void Ready()
        {
            PlayerId? managedPlayer = ReferenceItems.BattleSettings.ManagedPlayer;

            if (managedPlayer == null)
            {
                Play();
            }
            else
            {
                PlayerReady?.Invoke(this, new EventArgs<PlayerId>((PlayerId)managedPlayer));    
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
                .First(x => x.Player == ReferenceItems.BattleSettings.ManagedPlayer)
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
                if (_currentPlayStep.Finished)
                {
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

        private PlayStep PlayNextStep()
        {
            BattleAction[] actions = _actionQueueControllers.Select(x => x.Dequeue()).Where(x => x != null).ToArray();

            if (actions.Length == 0)
            {
                return null;
            }

            var playStep = new PlayStep(actions);

            playStep.Play(ReferenceItems.ShipControllers, ReferenceItems.BattleZoneDescription);

            return playStep;
        }
    }
}