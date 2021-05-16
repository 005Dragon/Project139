using System;
using System.Linq;
using Code.Battle.ActionCreators;
using Code.Utils;
using UnityEngine;

namespace Code.Battle.Actions
{
    public abstract class BattleAction : IDisposable
    {
        public PlayerSide PlayerSide { get; }
        
        public BattleActionType ActionType { get; }
        
        public Sprite Sprite { get; }
        
        public float EnergyCost { get; }
        
        public bool Started { get; protected set; }

        public bool Finished => Started && !SelfShip.ActionInProcess;

        protected IBattleShip SelfShip { get; private set; }
        protected IBattleShip EnemyShip { get; private set; }
        
        protected IBattleZone BattleZone { get; private set; }

        protected BattleAction(IBattleActionCreator creator)
        {
            PlayerSide = creator.PlayerSide;
            ActionType = creator.ActionType;
            Sprite = creator.Sprite;
            EnergyCost = creator.EnergyCost;
        }

        public void Play(IBattleShip[] shipControllers, IBattleZone battleZone)
        {
            SelfShip = shipControllers.First(x => x.PlayerSide == PlayerSide);
            EnemyShip = shipControllers.First(x => x.PlayerSide == PlayerSide.GetAnother());
            
            BattleZone = battleZone;

            PlayCore();
            
            Started = true;
        }

        protected abstract void PlayCore();
        
        public virtual void Dispose()
        {
        }
    }
}