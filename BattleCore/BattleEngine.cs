using System;
using System.Linq;
using BattleCore.Actions;
using BattleCore.Log;
using BattleCore.Utils;

namespace BattleCore
{
    public class BattleEngine
    {
        public int RoundLimit { get; set; } = 100;
        
        private readonly IBattlePlayer[] _players;
        private readonly IBattleShip[] _ships;
        private readonly IBattleZone _battleZone;
        private readonly IBattleActionQueue[] _battleActionQueues;
        private readonly IBattleActionCreator[] _battleActionCreators;
        private readonly IBattleLogger _logger;
        private readonly IRandom _random;
        
        private int _roundIndex;
        private bool _roundAlreadyStarted;

        public BattleEngine(
            IBattlePlayer[] players, 
            IBattleShip[] ships,
            IBattleZone battleZone,
            IBattleActionQueue[] battleActionQueues,
            IBattleActionCreator[] battleActionCreators,
            IBattleLogger logger, 
            IRandom random)
        {
            _players = players;
            _ships = ships;
            _battleZone = battleZone;
            _battleActionQueues = battleActionQueues;
            _battleActionCreators = battleActionCreators;
            _logger = logger;
            _random = random;
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
                var playerBattleActionCreators = new IBattleActionCreator[_battleActionCreators.Length];

                for (int i = 0; i < playerBattleActionCreators.Length; i++)
                {
                    playerBattleActionCreators[i] = (IBattleActionCreator) _battleActionCreators[i].Clone();
                }
                
                battlePlayer.AddEnableBattleActionCreators(playerBattleActionCreators);
            }

            foreach (IBattleShip ship in _ships)
            {
                ship.ShipDestroy += OnShipDestroy;
            }

            _logger.LogMessage(BattleLoggerMessageType.Info, "Battle initialized.");
        }

        public bool PlayNextRound()
        {
            if (_roundAlreadyStarted)
            {
                return true;
            }

            if (_ships.Any(x => x.Destroyed) || _roundIndex > RoundLimit)
            {
                return false;
            }
            
            _roundAlreadyStarted = true;

            _roundIndex++;
            
            _logger.LogRound(_roundIndex, "Initialized.");

            foreach (IBattleShip ship in _ships)
            {
                ship.SetEnergy(ship.MaxEnergy);
            }

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
                var round = new BattleRound(
                    _roundIndex,
                    _battleActionQueues,
                    _ships,
                    _battleZone,
                    _logger,
                    _random
                );

                round.Finished += OnRoundFinished;
            
                round.Play();
            }
        }

        private void OnRoundFinished(object sender, EventArgs eventArgs)
        {
            foreach (IBattlePlayer battlePlayer in _players)
            {
                battlePlayer.Sleep();
            }

            _roundAlreadyStarted = false;
        }
        
        private void OnShipDestroy(object sender, EventArgs eventArgs)
        {
            var ship = (IBattleShip) sender;
            
            foreach (IBattlePlayer battlePlayer in _players)
            {
                battlePlayer.Sleep();
            }
            
            _logger.LogWinner(ship.PlayerSide.GetAnother(), $"Step {_roundIndex}.");
        }
    }
}