using System;
using BattleCore.Utils;

namespace BattleCore.Players
{
    public abstract class BattlePlayerBase : IBattlePlayer
    {
        public PlayerSide PlayerSide { get; }

        public bool IsReady
        {
            get => _isReady;
            set
            {
                _isReady = value;
                
                ReadyChange?.Invoke(this, new EventArgs<bool>(_isReady));
            }
        }
        
        public abstract event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction;
        public event EventHandler<EventArgs<bool>> ReadyChange;

        private bool _isReady;

        protected BattlePlayerBase(PlayerSide playerSide)
        {
            PlayerSide = playerSide;
        }

        public abstract void AddEnableBattleActionCreators(IBattleActionCreator[] battleActionCreators);

        public virtual void Sleep()
        {
            IsReady = false;
        }

        public abstract void Wake();
    }
}