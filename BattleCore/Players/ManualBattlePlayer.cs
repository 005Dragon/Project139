using System;
using BattleCore.UI;
using BattleCore.Utils;

namespace BattleCore.Players
{
    public class ManualBattlePlayer : BattlePlayerBase
    {
        public override event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        
        private readonly IUiBattleActionBar _battleActionBar;

        private bool _sleep;

        public ManualBattlePlayer(PlayerSide playerSide, IUiBattleActionBar battleActionBar, IUiPlayerReady playerReady)
            : base(playerSide)
        {
            _battleActionBar = battleActionBar;
            playerReady.Ready += OnPlayerReady;
        }

        public override void AddEnableBattleActionCreators(IBattleActionCreator[] battleActionCreators)
        {
            _battleActionBar.AddEnableBattleActionCreators(PlayerSide, battleActionCreators);

            _battleActionBar.CreateBattleAction += OnBattleActionBarCreateBattleAction;
        }

        public override void Sleep()
        {
            base.Sleep();
            
            _sleep = true;
            _battleActionBar.Visible = false;
        }

        public override void Wake()
        {
            _sleep = false;
            _battleActionBar.Visible = true;
        }
        
        private void OnBattleActionBarCreateBattleAction(object sender, EventArgs<IBattleActionCreator> eventArgs)
        {
            CreateBattleAction?.Invoke(this, new EventArgs<IBattleActionCreator>(eventArgs.Value));
        }
        
        private void OnPlayerReady(object sender, EventArgs eventArgs)
        {
            if (!_sleep)
            {
                IsReady = true;
            }
        }
    }
}