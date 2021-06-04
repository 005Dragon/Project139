using System;
using System.Collections.Generic;
using System.Linq;
using BattleCore.Actions;

namespace BattleCore
{
    public class BattleStep
    {
        private class BattleActionGroup
        {
            public event EventHandler Finished;

            private readonly BattleAction[] _actions;

            private int _finishedActionCount;

            public BattleActionGroup(BattleAction[] actions)
            {
                _actions = actions;
            }

            public void Play(IBattleShip[] shipControllers, IBattleZone battleZone)
            {
                foreach (BattleAction battleAction in _actions)
                {
                    if (battleAction == null)
                    {
                        _finishedActionCount++;
                        
                        continue;
                    }

                    battleAction.Finished += OnBattleActionFinished;
                    
                    battleAction.Play(shipControllers, battleZone);
                }
                
                CheckFinished();
            }

            private void OnBattleActionFinished(object sender, EventArgs eventArgs)
            {
                var battleAction = (BattleAction) sender;
                
                battleAction.Finished -= OnBattleActionFinished;

                _finishedActionCount++;
                
                CheckFinished();
            }

            private void CheckFinished()
            {
                if (_finishedActionCount == _actions.Length)
                {
                    Finished?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        public event EventHandler Finished;

        private readonly int _index;

        private readonly IBattleShip[] _shipControllers;
        private readonly IBattleZone _battleZone;
        
        private readonly Queue<BattleActionGroup> _battleActionGroups = new Queue<BattleActionGroup>();
        
        public BattleStep(int index, BattleAction[] actions, IBattleShip[] shipControllers, IBattleZone battleZone)
        {
            _index = index;

            _shipControllers = shipControllers;
            _battleZone = battleZone;

            _battleActionGroups.Enqueue(new BattleActionGroup(actions.Where(x => x.ActionType == BattleActionType.Internal).ToArray()));
            _battleActionGroups.Enqueue(new BattleActionGroup(actions.Where(x => x.ActionType == BattleActionType.Defense).ToArray()));
            _battleActionGroups.Enqueue(new BattleActionGroup(actions.Where(x => x.ActionType == BattleActionType.Attack).ToArray()));

            foreach (BattleActionGroup battleActionGroup in _battleActionGroups)
            {
                battleActionGroup.Finished += OnBattleActionGroupFinished;
            }
        }

        private void OnBattleActionGroupFinished(object sender, EventArgs eventArgs)
        {
            var battleActionGroup = (BattleActionGroup) sender;

            battleActionGroup.Finished -= OnBattleActionGroupFinished;

            if (_battleActionGroups.Count > 0)
            {
                _battleActionGroups.Dequeue().Play(_shipControllers, _battleZone);    
            }
            else
            {
                Finished?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Play()
        {
            if (_battleActionGroups.Count > 0)
            {
                _battleActionGroups.Dequeue().Play(_shipControllers, _battleZone);
            }
            else
            {
                Finished?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}