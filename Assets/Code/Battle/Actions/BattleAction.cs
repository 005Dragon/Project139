using System;
using System.Linq;
using Code.Battle.ActionCreators;
using Code.Battle.Log;
using Code.Utils;
using UnityEngine;

namespace Code.Battle.Actions
{
    public abstract class BattleAction
    {
        public event EventHandler Finished;
        
        public int Id { get; set; }
        
        public PlayerSide PlayerSide { get; }
        
        public BattleActionType ActionType { get; }
        
        public Sprite Sprite { get; }
        
        public float EnergyCost { get; }
        
        protected IBattleLogger Logger { get; }

        protected IBattleShip SelfShip { get; private set; }
        protected IBattleShip EnemyShip { get; private set; }
        
        protected IBattleZone BattleZone { get; private set; }

        protected BattleAction(IBattleActionCreator creator)
        {
            PlayerSide = creator.PlayerSide;
            ActionType = creator.ActionType;
            Sprite = creator.Sprite;
            EnergyCost = creator.EnergyCost;
            Logger = creator.Logger;
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