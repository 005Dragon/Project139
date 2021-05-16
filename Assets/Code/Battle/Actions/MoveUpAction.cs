using Code.Battle.ActionCreators;

namespace Code.Battle.Actions
{
    public class MoveUpAction : BattleAction
    {
        public MoveUpAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
            SelfShip.ChangeBattleZone(Direction4.Up);
        }
    }
}