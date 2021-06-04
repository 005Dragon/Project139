using System.Collections.Generic;
using Code.Battle.Core;

namespace Code.Services
{
    public interface IBattleShipsService
    {
        IEnumerable<IBattleShip> GetBattleShips();
    }
}