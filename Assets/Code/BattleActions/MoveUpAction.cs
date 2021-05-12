using Code.BattleActionCreators;

namespace Code.BattleActions
{
    public class MoveUpAction : BattleAction
    {
        public MoveUpAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
            SelfShipController.ChangeBattleZone(Direction4.Up);
        }
    }
}