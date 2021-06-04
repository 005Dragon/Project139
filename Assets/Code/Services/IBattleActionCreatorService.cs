using System.Collections.Generic;
using Code.Battle.Core;

namespace Code.Services
{
    public interface IBattleActionCreatorService
    {
        IEnumerable<IBattleActionCreator> GetBattleActionCreators();
    }
}