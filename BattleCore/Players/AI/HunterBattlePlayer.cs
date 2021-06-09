using System;
using System.Collections.Generic;
using System.Linq;
using BattleCore.Actions;
using BattleCore.Utils;

namespace BattleCore.Players.AI
{
    public class HunterBattlePlayer : IBattlePlayer
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        public event EventHandler Ready;
        
        public PlayerSide PlayerSide { get; }
        
        public bool IsReady { get; private set; }

        private IBattleActionCreator[] _battleActionCreators;
        
        private readonly IRandom _random;
        private readonly IBattleZone _battleZone;
        private readonly IBattleShip _selfShip;

        public HunterBattlePlayer(PlayerSide playerSide, IRandom random, IBattleZone battleZone, IBattleShip selfShip)
        {
            _random = random;
            _battleZone = battleZone;
            _selfShip = selfShip;
            PlayerSide = playerSide;
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
            BattleActionId? lastAction = null;
            
            while (_selfShip.Energy > 0)
            {
                lastAction = GenerateBattleAction(lastAction);
            }

            IsReady = true;
            
            Ready?.Invoke(this, EventArgs.Empty);
        }
        
        private BattleActionId GenerateBattleAction(BattleActionId? lastAction)
        {
            int oppositeDifference = GetOppositeDifference(lastAction);
            
            IBattleActionCreator actionCreator;

            if (oppositeDifference == 0)
            {
                actionCreator = _battleActionCreators.First(x => x.ActionId == BattleActionId.DirectShot);
            }
            else
            {
                if (oppositeDifference > 0)
                {
                    actionCreator = _battleActionCreators.First(x => x.ActionId == BattleActionId.MoveDown);
                }
                else
                {
                    actionCreator = _battleActionCreators.First(x => x.ActionId == BattleActionId.MoveUp);
                }
            }

            CreateBattleAction?.Invoke(this, new EventArgs<IBattleActionCreator>(actionCreator));

            return actionCreator.ActionId;
        }

        private int GetOppositeDifference(BattleActionId? lastAction)
        {
            int selfShipZoneIndex = _battleZone.GetShipBattleZoneField(PlayerSide).Index;
            int enemyShipZoneIndex = _battleZone.GetShipBattleZoneField(PlayerSide.GetAnother()).Index;

            int result = enemyShipZoneIndex - selfShipZoneIndex;

            if (lastAction == BattleActionId.MoveUp)
            {
                result++;
            }

            if (lastAction == BattleActionId.MoveDown)
            {
                result--;
            }

            return result;
        }
    }
}