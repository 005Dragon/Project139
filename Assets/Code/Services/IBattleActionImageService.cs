using BattleCore.Actions;
using UnityEngine;

namespace Code.Services
{
    public interface IBattleActionImageService
    {
        Sprite GetSprite(BattleActionId battleActionId);
    }
}