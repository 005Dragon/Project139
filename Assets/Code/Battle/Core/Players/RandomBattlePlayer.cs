﻿using System;
using System.Collections.Generic;
using System.Linq;
using Code.Utils;

namespace Code.Battle.Core.Players
{
    public class RandomBattlePlayer : IBattlePlayer
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        public event EventHandler Ready;
        
        public PlayerSide PlayerSide { get; }
        
        public bool IsReady { get; private set; }

        private IBattleActionCreator[] _battleActionCreators;

        private readonly IRandom _random;
        private readonly IBattleZone _battleZone;
        private readonly IBattleShip _selfShip;

        public RandomBattlePlayer(PlayerSide playerSide, IRandom random, IBattleZone battleZone, IBattleShip selfShip)
        {
            PlayerSide = playerSide;
            _random = random;
            _battleZone = battleZone;
            _selfShip = selfShip;
        }

        public void AddEnableBattleActionCreators(IEnumerable<IBattleActionCreator> battleActionCreators)
        {
            _battleActionCreators = battleActionCreators.ToArray();
        }

        public void Sleep()
        {
            IsReady = false;
        }

        public void Wake()
        {
            while (_selfShip.Energy > 0)
            {
                GenerateBattleAction();    
            }

            IsReady = true;
            
            Ready?.Invoke(this, EventArgs.Empty);
        }
        
        private void GenerateBattleAction()
        {
            IBattleActionCreator actionCreator = _battleActionCreators[_random.Range(0, _battleActionCreators.Length)];

            if (actionCreator is ITargetBattleActionCreator targetBattleActionCreator)
            {
                IBattleZoneField[] enableTargets = targetBattleActionCreator.GetEnableTargets(PlayerSide, _battleZone).ToArray();

                IBattleZoneField target = enableTargets[_random.Range(0, enableTargets.Length)];
                
                targetBattleActionCreator.SetTargets(target.ToSingleElementEnumerable());
            }

            CreateBattleAction?.Invoke(this, new EventArgs<IBattleActionCreator>(actionCreator));
        }
    }
}