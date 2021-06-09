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

            private readonly IBattleZone _battleZone;
            private readonly IBattleShip[] _ships;

            public BattleActionGroup(BattleAction[] actions, IBattleZone battleZone, IBattleShip[] ships)
            {
                _actions = actions;
                _battleZone = battleZone;
                _ships = ships;
            }

            public void Play()
            {
                foreach (BattleAction battleAction in _actions)
                {
                    if (battleAction == null)
                    {
                        _finishedActionCount++;
                        
                        continue;
                    }

                    battleAction.Finished += OnBattleActionFinished;
                    
                    battleAction.Play(_ships, _battleZone);
                }

                if (CheckFinished(_ships))
                {
                    Finished?.Invoke(this, EventArgs.Empty);
                }
            }

            private void OnBattleActionFinished(object sender, EventArgs eventArgs)
            {
                var battleAction = (BattleAction) sender;
                
                battleAction.Finished -= OnBattleActionFinished;

                _finishedActionCount++;
                
                if (CheckFinished(_ships))
                {
                    Finished?.Invoke(this, EventArgs.Empty);
                }
            }

            private bool CheckFinished(IBattleShip[] ships)
            {
                if (ships.Any(x => x.Destroyed))
                {
                    return true;
                }
                
                if (_finishedActionCount == _actions.Length)
                {
                    return true;
                }

                return false;
            }
        }
        
        public event EventHandler Finished;

        private readonly int _index;
        
        private readonly Queue<BattleActionGroup> _battleActionGroups = new Queue<BattleActionGroup>();
        
        public BattleStep(int index, BattleAction[] actions, IBattleShip[] ships, IBattleZone battleZone)
        {
            _index = index;

            _battleActionGroups.Enqueue(
                new BattleActionGroup(actions.Where(x => x.ActionType == BattleActionType.Internal).ToArray(), battleZone, ships)
            );

            _battleActionGroups.Enqueue(
                new BattleActionGroup(actions.Where(x => x.ActionType == BattleActionType.Defense).ToArray(), battleZone, ships)
            );

            _battleActionGroups.Enqueue(
                new BattleActionGroup(actions.Where(x => x.ActionType == BattleActionType.Attack).ToArray(), battleZone, ships)
            );
            
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
                _battleActionGroups.Dequeue().Play();    
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
                _battleActionGroups.Dequeue().Play();
            }
            else
            {
                Finished?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}