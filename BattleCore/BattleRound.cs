using System;
using System.Linq;
using BattleCore.Actions;
using BattleCore.Log;

namespace BattleCore
{
    public class BattleRound
    {
        public event EventHandler Finished; 
        
        private readonly IBattleActionQueue _leftBattleActionQueues;
        private readonly IBattleActionQueue _rightBattleActionQueue;

        private readonly IBattleShip[] _ships;
        private readonly IBattleZone _battleZone;
        private readonly IBattleLogger _logger;
        private readonly int _roundIndex;
        private readonly IRandom _random;

        private BattleAction _leftSideBattleAction;
        private BattleAction _rightSideBattleAction;

        public BattleRound(
            int roundIndex,
            IBattleActionQueue[] battleActionQueues, 
            IBattleShip[] ships, 
            IBattleZone battleZone, 
            IBattleLogger logger, 
            IRandom random)
        {
            _leftBattleActionQueues = battleActionQueues.First(x => x.PlayerSide == PlayerSide.Left);
            _rightBattleActionQueue = battleActionQueues.First(x => x.PlayerSide == PlayerSide.Right);
            _ships = ships;
            _battleZone = battleZone;
            _logger = logger;
            _roundIndex = roundIndex;
            _random = random;
        }

        public void Play()
        {
            _logger.LogRound(_roundIndex, "Start.");

            PlayNextAction();
        }

        private void PlayNextAction()
        {
            if (_leftSideBattleAction == null)
            {
                _leftSideBattleAction = _leftBattleActionQueues.Dequeue();
            }

            if (_rightSideBattleAction == null)
            {
                _rightSideBattleAction = _rightBattleActionQueue.Dequeue();    
            }
            
            if (_leftSideBattleAction == null && _rightSideBattleAction == null)
            {
                Finish();
                
                return;
            }

            if (_leftSideBattleAction == null)
            {
                PlayAction(ref _rightSideBattleAction);
                
                return;
            }

            if (_rightSideBattleAction == null)
            {
                PlayAction(ref _leftSideBattleAction);
                
                return;
            }

            bool leftActionPriority = _random.Range(0, 2) == 1;

            if (leftActionPriority)
            {
                if (_leftSideBattleAction.Duration < _rightSideBattleAction.Duration)
                {
                    _rightSideBattleAction.Duration -= _leftSideBattleAction.Duration;
                
                    PlayAction(ref _leftSideBattleAction);
                }
                else
                {
                    _leftSideBattleAction.Duration -= _rightSideBattleAction.Duration;
                    
                    PlayAction(ref _rightSideBattleAction);
                }
            }
            else
            {
                if (_rightSideBattleAction.Duration < _leftSideBattleAction.Duration)
                {
                    _leftSideBattleAction.Duration -= _rightSideBattleAction.Duration;
                    
                    PlayAction(ref _rightSideBattleAction);
                }
                else
                {
                    _rightSideBattleAction.Duration -= _leftSideBattleAction.Duration;
                    
                    PlayAction(ref _leftSideBattleAction);
                }
            }
        }

        private void PlayAction(ref BattleAction action)
        {
            action.Finished += OnActionFinished;
            
            action.Play(_ships, _battleZone);
        }

        private void OnActionFinished(object sender, EventArgs eventArgs)
        {
            var action = (BattleAction) sender;

            if (action == _leftSideBattleAction)
            {
                _leftSideBattleAction = null;
            }

            if (action == _rightSideBattleAction)
            {
                _rightSideBattleAction = null;
            }
            
            action.Finished -= OnActionFinished;

            if (_ships.Any(x => x.Destroyed))
            {
                Finish();
                
                return;
            }

            bool noMoreActions =
                _leftSideBattleAction == null &&
                _rightSideBattleAction == null &&
                _leftBattleActionQueues.IsEmpty &&
                _rightBattleActionQueue.IsEmpty; 
            
            if (noMoreActions)
            {
                Finish();
                
                return;
            }
            
            PlayNextAction();
        }

        private void Finish()
        {
            _leftBattleActionQueues.Clear();
            _rightBattleActionQueue.Clear();
            
            _logger.LogRound(_roundIndex, "End.");
            
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}