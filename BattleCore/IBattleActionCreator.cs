using System;
using BattleCore.Actions;
using BattleCore.Log;

namespace BattleCore
{
    public interface IBattleActionCreator : ICloneable
    {
        BattleActionId ActionId { get; }
        
        BattleActionType ActionType { get; }
        
        IBattleLogger Logger { get; set; }
        
        float EnergyCost { get; }
        
        float Duration { get; }
        
        BattleAction Create(PlayerSide playerSide);
    }
}