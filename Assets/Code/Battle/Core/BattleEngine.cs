using System;
using System.Linq;
using Code.Battle.Core.Actions;
using Code.Battle.Core.Log;
using Code.Utils;

namespace Code.Battle.Core
{
    public class BattleEngine : IManagedInitializable
    {
        private readonly IBattlePlayer[] _players;
        private readonly IBattleShip[] _ships;
        private readonly IBattleZone _battleZone;
        private readonly IBattleActionQueue[] _battleActionQueues;
        private readonly IBattleActionCreator[] _battleActionCreators;
        private readonly IBattleLogger _logger;

        private int _stepIndex;

        public BattleEngine(
            IBattlePlayer[] players, 
            IBattleShip[] ships,
            IBattleZone battleZone,
            IBattleActionQueue[] battleActionQueues,
            IBattleActionCreator[] battleActionCreators,
            IBattleLogger logger)
        {
            _players = players;
            _ships = ships;
            _battleZone = battleZone;
            _battleActionQueues = battleActionQueues;
            _battleActionCreators = battleActionCreators;
            _logger = logger;
        }

        public void Initialize()
        {
            foreach (IBattlePlayer player in _players)
            {
                player.CreateBattleAction += OnCreateBattleAction;
                player.Ready += OnPlayerReady;
            }

            foreach (IBattleActionCreator battleActionCreator in _battleActionCreators)
            {
                battleActionCreator.Logger = _logger;
            }

            foreach (IBattlePlayer battlePlayer in _players)
            {
                battlePlayer.AddEnableBattleActionCreators(_battleActionCreators);
            }

            foreach (IBattleShip ship in _ships)
            {
                ship.ShipDestroy += OnShipDestroy;
            }

            _logger.LogMessage(BattleLoggerMessageType.Info, "Battle initialized.");
        }

        public void Play()
        {
            _logger.LogMessage(BattleLoggerMessageType.Info, "Battle play.");
            
            foreach (IBattlePlayer battlePlayer in _players)
            {
                battlePlayer.Wake();
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
            
            _logger.LogActionMessage(BattleLoggerMessageType.Info, battleAction, "Created.");
        }

        private void OnPlayerReady(object sender, EventArgs eventArgs)
        {
            _logger.LogPlayerMessage(BattleLoggerMessageType.Info, ((IBattlePlayer)sender).PlayerSide, "Ready.");
            
            bool allPlayersReady = _players.All(x => x.IsReady);

            if (allPlayersReady)
            {
                foreach (IBattleShip ship in _ships)
                {
                    _logger.LogShipMessage(BattleLoggerMessageType.Info, ship, string.Empty);
                }
                
                _stepIndex++;
            
                _logger.LogMessage(BattleLoggerMessageType.Info, $"Start {_stepIndex} step.");
                
                if (TryGetNextStep(out BattleStep step))
                {
                    PlayStep(step);
                }
                else
                {
                    FinishStep();
                }
            }
        }

        private void PlayStep(BattleStep step)
        {
            foreach (IBattlePlayer player in _players)
            {
                player.Sleep();
            }

            step.Finished += OnStepFinished;
            
            step.Play();
        }

        private void OnStepFinished(object sender, EventArgs eventArgs)
        {
            var step = (BattleStep) sender;

            step.Finished -= OnStepFinished;

            if (TryGetNextStep(out var nextStep))
            {
                PlayStep(nextStep);
            }
            else
            {
                FinishStep();
            }
        }
        
        private void OnShipDestroy(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;
            
            foreach (IBattlePlayer battlePlayer in _players)
            {
                battlePlayer.Sleep();
            }
            
            _logger.LogPlayerMessage(BattleLoggerMessageType.Info, ship.PlayerSide, "Destroy.");
            _logger.LogPlayerMessage(BattleLoggerMessageType.Info, ship.PlayerSide.GetAnother(), "Winner!");
        }

        private void FinishStep()
        {
            _logger.LogMessage(BattleLoggerMessageType.Info, $"Step {_stepIndex} finished.");
            
            foreach (IBattleShip ship in _ships)
            {
                ship.SetEnergy(ship.MaxEnergy);
            }
            
            foreach (IBattlePlayer player in _players)
            {
                player.Wake();
            }
        }

        private bool TryGetNextStep(out BattleStep step)
        {
            BattleAction[] actions = _battleActionQueues.Select(x => x.Dequeue()).Where(x => x != null).ToArray();

            if (actions.Length == 0)
            {
                step = default;
                
                return false;
            }

            step = new BattleStep(_stepIndex, actions, _ships, _battleZone);

            return true;
        }
    }
}