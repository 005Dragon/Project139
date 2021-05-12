using Code.BattleActionCreators;

namespace Code.BattleActions
{
    public class MoveDownAction : BattleAction
    {
        public MoveDownAction(IBattleActionCreator creator) : base(creator)
        {
        }

        protected override void PlayCore()
        {
            SelfShipController.ChangeBattleZone(Direction4.Down);
        }
    }
}