﻿using System;
using Code.Battle.ActionCreators;
using Code.Utils;

namespace Code.Battle
{
    public interface IBattlePlayer
    {
        public PlayerSide PlayerSide { get; }
        
        bool IsReady { get; }

        event EventHandler<EventArgs<IBattleActionCreator>> CreateBattleAction; 
        event EventHandler Ready;
        
        void Sleep();

        void Wake();
    }
}