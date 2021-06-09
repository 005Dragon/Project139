﻿using System;
using System.Linq;
using BattleCore;
using BattleCore.Players;
using BattleCore.Players.AI;
using BattleCore.UI;

namespace Code.Battle
{
    public class BattlePlayerCreator
    {
        private readonly IRandom _random;
        private readonly IBattleZone _battleZone;
        private readonly IUiBattleActionBar _battleActionBar;
        private readonly IUiPlayerReady _playerReady;
        private readonly IBattleShip[] _ships;

        public BattlePlayerCreator(
            IRandom random, 
            IBattleZone battleZone, 
            IUiBattleActionBar battleActionBar, 
            IUiPlayerReady playerReady, 
            IBattleShip[] ships)
        {
            _random = random;
            _battleZone = battleZone;
            _battleActionBar = battleActionBar;
            _playerReady = playerReady;
            _ships = ships;
        }

        public IBattlePlayer Create(PlayerSide playerSide, PlayerManagementType playerManagementType)
        {
            switch (playerManagementType)
            {
                case PlayerManagementType.None: return CreateDefaultBattlePlayer(playerSide);
                case PlayerManagementType.RandomAi: return CreateRandomBattlePlayer(playerSide);
                case PlayerManagementType.Manual: return CreateManualBattlePlayer(playerSide);
                case PlayerManagementType.HunterAi: return CreateHunterBattlePlayer(playerSide);
                default: throw new ArgumentOutOfRangeException(nameof(playerManagementType), playerManagementType, null);
            }
        }

        private IBattlePlayer CreateHunterBattlePlayer(PlayerSide playerSide)
        {
            return new HunterBattlePlayer(playerSide, _random, _battleZone, _ships.First(x => x.PlayerSide == playerSide));
        }

        private IBattlePlayer CreateDefaultBattlePlayer(PlayerSide playerSide)
        {
            return new DefaultBattlePlayer(playerSide);
        }

        private IBattlePlayer CreateRandomBattlePlayer(PlayerSide playerSide)
        {
            return new RandomBattlePlayer(playerSide, _random, _battleZone, _ships.First(x => x.PlayerSide == playerSide));
        }

        private IBattlePlayer CreateManualBattlePlayer(PlayerSide playerSide)
        {
            return new ManualBattlePlayer(playerSide, _battleActionBar, _playerReady);
        }
    }
}