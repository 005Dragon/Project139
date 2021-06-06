using System.Collections.Generic;
using BattleCore;

namespace Code.Services
{
    public interface IBattleShipsService
    {
        IEnumerable<IBattleShip> GetBattleShips();
    }
}