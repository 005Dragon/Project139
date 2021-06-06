using System.Collections.Generic;
using BattleCore;

namespace Code.Services
{
    public interface IBattleActionCreatorService
    {
        IEnumerable<IBattleActionCreator> GetBattleActionCreators();
    }
}