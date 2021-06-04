using System;
using System.Collections.Generic;
using BattleCore.UI;
using BattleCore.Utils;

namespace BattleCore.Players
{
    public class ManualBattlePlayer : IBattlePlayer
    {
        public event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        public event EventHandler Ready;
        
        public PlayerSide PlayerSide { get; }
        
        public bool IsReady { get; private set; }

        private readonly IUiBattleActionBar _battleActionBar;

        private bool _sleep;

        public ManualBattlePlayer(PlayerSide playerSide, IUiBattleActionBar battleActionBar, IUiPlayerReady playerReady)
        {
            PlayerSide = playerSide;
            _battleActionBar = battleActionBar;

            playerReady.Ready += OnPlayerReady;
        }

        public void AddEnableBattleActionCreators(IEnumerable<IBattleActionCreator> battleActionCreators)
        {
            _battleActionBar.AddEnableBattleActionCreators(PlayerSide, battleActionCreators);

            _battleActionBar.CreateBattleAction += OnBattleActionBarCreateBattleAction;
        }

        public void Sleep()
        {
            _sleep = true;
            IsReady = false;
            _battleActionBar.Visible = false;
        }

        public void Wake()
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
                
                Ready?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}