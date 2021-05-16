using Code.Battle.ActionCreators;

namespace Code.Battle.Actions
{
    public class MoveDownAction : BattleAction
    {
        public MoveDownAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
            SelfShip.ChangeBattleZone(Direction4.Down);
        }
    }
}