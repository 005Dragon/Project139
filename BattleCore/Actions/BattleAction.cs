using System;
using System.Linq;
using BattleCore.Log;
using BattleCore.Utils;

namespace BattleCore.Actions
{
    public abstract class BattleAction
    {
        public event EventHandler Finished;
        
        public BattleActionId Id { get; }
        
        public PlayerSide PlayerSide { get; }
        
        public BattleActionType ActionType { get; }
        
        public float EnergyCost { get; }
        
        public float Duration { get; set; }
        
        protected IBattleLogger Logger { get; }

        protected IBattleShip SelfShip { get; private set; }
        protected IBattleShip EnemyShip { get; private set; }
        
        protected IBattleZone BattleZone { get; private set; }

        protected BattleAction(PlayerSide playerSide, IBattleActionCreator creator)
        {
            PlayerSide = playerSide;
            ActionType = creator.ActionType;
            EnergyCost = creator.EnergyCost;
            Logger = creator.Logger;
            Id = creator.ActionId;
            Duration = creator.Duration;
        }

        public void Play(IBattleShip[] shipControllers, IBattleZone battleZone)
        {
            SelfShip = shipControllers.First(x => x.PlayerSide == PlayerSide);
            EnemyShip = shipControllers.First(x => x.PlayerSide == PlayerSide.GetAnother());
            
            BattleZone = battleZone;

            PlayCore();
        }

        protected abstract void PlayCore();

        protected void Finish()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}