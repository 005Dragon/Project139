using UnityEngine;

namespace Code
{
    public class AnimationVisibleController : MonoBehaviour, IManagedInitializable
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        public void Show()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;    
            }

            if (_animator != null)
            {
                _animator.enabled = true;
                _animator.Play(0);    
            }
        }

        public void Hide()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = false;    
            }

            if (_animator != null)
            {
                _animator.enabled = false;    
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
