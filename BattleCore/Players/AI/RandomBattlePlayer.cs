using System;
using System.Collections.Generic;
using System.Linq;
using BattleCore.Utils;

namespace BattleCore.Players.AI
{
    public class RandomBattlePlayer : BattlePlayerBase
    {
        public override event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        private IBattleActionCreator[] _battleActionCreators;

        private readonly IRandom _random;
        private readonly IBattleZone _battleZone;
        private readonly IBattleShip _selfShip;

        public RandomBattlePlayer(PlayerSide playerSide, IRandom random, IBattleZone battleZone, IBattleShip selfShip)
            : base(playerSide)
        {
            _random = random;
            _battleZone = battleZone;
            _selfShip = selfShip;
        }

        public override void AddEnableBattleActionCreators(IBattleActionCreator[] battleActionCreators)
        {
            _battleActionCreators = battleActionCreators.ToArray();
        }

        public override void Wake()
        {
            while (_selfShip.Energy > 0)
            {
                GenerateBattleAction();    
            }

            IsReady = true;
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