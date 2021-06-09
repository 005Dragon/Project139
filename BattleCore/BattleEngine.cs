using System;
using System.Linq;
using BattleCore.Actions;
using BattleCore.Log;
using BattleCore.Utils;

namespace BattleCore
{
    public class BattleEngine
    {
        public bool Finished { get; private set; } = true;
        
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

        public bool Play()
        {
            if (!Finished)
            {
                return false;
            }

            Finished = false;
            
            if (_ships.Any(x => x.Destroyed))
            {
                return false;
            }
            
            _stepIndex++;
            
            _logger.LogStep(_stepIndex, string.Empty);
            
            foreach (IBattleShip ship in _ships)
            {
                _logger.LogShipParameters(ship, _battleZone, string.Empty);
            }
            
            foreach (IBattlePlayer battlePlayer in _players)
            {
                battlePlayer.Wake();
            }

            return true;
        }

        private void OnCreateBattleAction(object sender, EventArgs<IBattleActionCreator> eventArgs)
        {
            var player = (IBattlePlayer) sender;
            
            TryCreateBattleAction(player.PlayerSide, eventArgs.Value);
        }
        
        private bool TryCreateBattleAction(PlayerSide playerSide, IBattleActionCreator creator)
        {
            if (!CanCreateBattleAction(playerSide, creator))
            {
                return false;
            }
            
            CreateBattleAction(playerSide, creator);

            return true;
        }
        
        private bool CanCreateBattleAction(PlayerSide playerSide, IBattleActionCreator creator)
        {
            float energyCost = creator.EnergyCost;

            IBattleShip ship = _ships.First(x => x.PlayerSide == playerSide);
            
            return energyCost <= ship.Energy;
        }
        
        private void CreateBattleAction(PlayerSide playerSide, IBattleActionCreator creator)
        {
            BattleAction battleAction = creator.Create(playerSide);

            IBattleShip ship = _ships.First(x => x.PlayerSide == playerSide);
            
            ship.SetEnergy(ship.Energy - battleAction.EnergyCost);

            IBattleActionQueue actionQueue = _battleActionQueues.First(x => x.PlayerSide == playerSide);
            
            actionQueue.Enqueue(battleAction);
            
            _logger.LogAction(battleAction, "Created.");
        }

        private void OnPlayerReady(object sender, EventArgs eventArgs)
        {
            _logger.LogMessage(BattleLoggerMessageType.Info, $"{((IBattlePlayer)sender).PlayerSide} ready.");
            
            bool allPlayersReady = _players.All(x => x.IsReady);

            if (allPlayersReady)
            {
                bool gotNextStep = TryGetNextStep(out BattleStep step);

                if (gotNextStep)
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
            
            _logger.LogWinner(ship.PlayerSide.GetAnother(), $"Step {_stepIndex}.");
        }

        private void FinishStep()
        {
            foreach (IBattleShip ship in _ships)
            {
                if (ship.Destroyed)
                {
                    return;
                }
                
                ship.SetEnergy(ship.MaxEnergy);
            }

            Finished = true;
        }

        private bool TryGetNextStep(out BattleStep step)
        {
            step = default;
            
            if (_ships.Any(x => x.Destroyed))
            {
                return false;
            }
            
            BattleAction[] actions = _battleActionQueues.Select(x => x.Dequeue()).Where(x => x != null).ToArray();

            if (actions.Length == 0)
            {
                return false;
            }

            step = new BattleStep(_stepIndex, actions, _ships, _battleZone);

            return true;
        }
    }
}