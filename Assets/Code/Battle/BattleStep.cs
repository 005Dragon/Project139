using System;
using System.Collections.Generic;
using System.Linq;
using Code.Battle.Actions;
using UnityEngine;

namespace Code.Battle
{
    public class BattleStep : IBattleStep, IDisposable
    {
        public event EventHandler Finished;
        
        private readonly BattleAction[] _actions;
        private readonly List<BattleAction[]> _battleActionBatches = new List<BattleAction[]>();

        private readonly IBattleShip[] _shipControllers;
        private readonly IBattleZone _battleZone;
        
        private bool _play;
        private int _internalStep = -1;
        
        public BattleStep(BattleAction[] actions, IBattleShip[] shipControllers, IBattleZone battleZone)
        {
            _actions = actions;
            _shipControllers = shipControllers;
            _battleZone = battleZone;

            _battleActionBatches.Add(_actions.Where(x => x.ActionType == BattleActionType.Internal).ToArray());
            _battleActionBatches.Add(_actions.Where(x => x.ActionType == BattleActionType.Defense).ToArray());
            _battleActionBatches.Add(_actions.Where(x => x.ActionType == BattleActionType.Attack).ToArray());
        }

        public void Play()
        {
            _play = true;
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

                        Finished?.Invoke(this, EventArgs.Empty);
                        
                        return;
                    }

                    foreach (BattleAction battleAction in _battleActionBatches[_internalStep])
                    {
                        battleAction.Play(_shipControllers, _battleZone);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (BattleAction action in _actions)
            {
                action?.Dispose();
            }
        }
    }
}