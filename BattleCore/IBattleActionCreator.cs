using BattleCore.Actions;
using BattleCore.Log;

namespace BattleCore
{
    public interface IBattleActionCreator
    {
        BattleActionId ActionId { get; }
        
        BattleActionType ActionType { get; }
        
        IBattleLogger Logger { get; set; }
        
        PlayerSide PlayerSide { get; set; }
        
        float EnergyCost { get; }
        
        BattleAction Create();
    }
}