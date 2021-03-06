using System;
using System.Linq;
using BattleCore.Actions;
using BattleCore.Utils;

namespace BattleCore.Players.AI
{
    public class HunterBattlePlayer : BattlePlayerBase
    {
        public override event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        private IBattleActionCreator[] _battleActionCreators;
        
        private readonly IRandom _random;
        private readonly IBattleZone _battleZone;
        private readonly IBattleShip _selfShip;

        public HunterBattlePlayer(PlayerSide playerSide, IRandom random, IBattleZone battleZone, IBattleShip selfShip)
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
            BattleActionId? lastAction = null;
            
            while (_selfShip.Energy > 0)
            {
                GenerateBattleAction(ref lastAction);
            }

            IsReady = true;
        }
        
        private void GenerateBattleAction(ref BattleActionId? lastAction)
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

            lastAction = actionCreator.ActionId;
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