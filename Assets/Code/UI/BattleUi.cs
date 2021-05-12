using UnityEngine;

namespace Code.UI
{
    public abstract class BattleUi : MonoBehaviour
    {
        protected IBattleReferenceItems ReferenceItems;
        
        protected abstract Canvas Canvas { get; set; }

        public virtual void Initialize(IBattleReferenceItems referenceItems)
        {
            ReferenceItems = referenceItems;
        }

        public virtual void Enable()
        {
            Canvas.enabled = true;
        }

        public virtual void Disable()
        {
            Canvas.enabled = false;
        }

        public virtual void Build()
        {
        }
    }
}