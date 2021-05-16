using System;
using System.Linq;
using Code.Battle.ActionCreators;
using Code.Battle.Actions;
using Code.Utils;

namespace Code.Battle
{
    public class Engine : IManagedInitializable
    {
        private readonly IBattlePlayer[] _players;
        private readonly IBattleShip[] _ships;
        private readonly IBattleActionQueue[] _battleActionQueues;
        private readonly Func<BattleAction[], IBattleStep> _stepCreator;

        public Engine(
            IBattlePlayer[] players, 
            IBattleShip[] ships, 
            IBattleActionQueue[] battleActionQueues,
            Func<BattleAction[], IBattleStep> stepCreator)
        {
            _players = players;
            _battleActionQueues = battleActionQueues;
            _stepCreator = stepCreator;
            _ships = ships;
        }

        public void Initialize()
        {
            foreach (IBattlePlayer player in _players)
            {
                player.CreateBattleAction += OnCreateBattleAction;
                player.Ready += OnPlayerReady;
            }
        }

        private void OnCreateBattleAction(object sender, EventArgs<IBattleActionCreator> eventArgs)
        {
            TryCreateBattleAction(eventArgs.Value);
        }
        
        private bool TryCreateBattleAction(IBattleActionCreator creator)
        {
            if (!CanCreateBattleAction(creator))
            {
                return false;
            }
            
            CreateBattleAction(creator);

            return true;
        }
        
        private bool CanCreateBattleAction(IBattleActionCreator creator)
        {
            float energyCost = creator.EnergyCost;

            IBattleShip ship = _ships.First(x => x.PlayerSide == creator.PlayerSide);
            
            return energyCost <= ship.Energy;
        }
        
        private void CreateBattleAction(IBattleActionCreator creator)
        {
            BattleAction battleAction = creator.Create();

            IBattleShip ship = _ships.First(x => x.PlayerSide == creator.PlayerSide);
            
            ship.SetEnergy(ship.Energy - battleAction.EnergyCost);

            IBattleActionQueue actionQueue = _battleActionQueues.First(x => x.PlayerSide == creator.PlayerSide);
            
            actionQueue.Enqueue(battleAction);
        }

        private void OnPlayerReady(object sender, EventArgs eventArgs)
        {
            bool allPlayersReady = _players.All(x => x.IsReady);

            if (allPlayersReady)
            {
                NextStep();
            }
        }

        private void NextStep()
        {
            foreach (IBattlePlayer player in _players)
            {
                player.Sleep();
            }
            
            BattleAction[] actions = _battleActionQueues.Select(x => x.Dequeue()).Where(x => x != null).ToArray();

            IBattleStep step = _stepCreator.Invoke(actions);

            step.Finished += OnStepFinished;
            
            step.Play();
        }

        private void OnStepFinished(object sender, EventArgs eventArgs)
        {
            var step = (IBattleStep) sender;

            step.Finished -= OnStepFinished;

            foreach (IBattlePlayer player in _players)
            {
                player.Wake();
            }
        }
    }
}