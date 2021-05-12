using Code.BattleActions;
using UnityEngine;

namespace Code.BattleActionCreators
{
    public class MoveUpActionCreator : BattleActionCreatorBase
    {
        public override BattleAction Create() => new MoveUpAction(this);
    }
}