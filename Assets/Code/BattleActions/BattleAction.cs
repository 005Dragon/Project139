using System;
using System.Linq;
using Code.BattleActionCreators;
using Code.Utils;
using UnityEngine;

namespace Code.BattleActions
{
    public abstract class BattleAction : IDisposable
    {
        public PlayerId Player { get; }
        
        public BattleActionType ActionType { get; }
        
        public Sprite Sprite { get; }
        
        public float EnergyCost { get; }
        
        public bool Started { get; protected set; }

        public bool Finished => Started && !SelfShipController.ActionInProcess;

        protected ShipController SelfShipController { get; private set; }
        protected ShipController EnemyShipController { get; private set; }
        
        protected BattleZoneDescription BattleZoneDescription { get; private set; }

        protected BattleAction(IBattleActionCreator creator)
        {
            Player = creator.Player;
            ActionType = creator.ActionType;
            Sprite = creator.Sprite;
            EnergyCost = creator.EnergyCost;
        }

        public void Play(ShipController[] shipControllers, BattleZoneDescription battleZoneDescription)
        {
            SelfShipController = shipControllers.First(x => x.Player == Player);
            EnemyShipController = shipControllers.First(x => x.Player == Player.GetAnother());
            
            BattleZoneDescription = battleZoneDescription;

            PlayCore();
            
            Started = true;
        }

        protected abstract void PlayCore();
        
        public virtual void Dispose()
        {
        }
    }
}