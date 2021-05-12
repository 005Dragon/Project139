using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public class DestroyController : MonoBehaviour
    {
        public event EventHandler Finished;
        
        [SerializeField]
        private GameObject _explosionTemplate;
        
        [SerializeField]
        private float _destroyRadius;
        
        [SerializeField] 
        private float _destroyDelay;

        [SerializeField]
        private int _explosionCount;

        [SerializeField]
        private float _explosionDelay;

        [SerializeField] 
        private int _batchSize;

        [SerializeField] 
        private float _minExplosionRadius;
        
        [SerializeField]
        private float _maxExplosionRadius;

        private float _lastExplosionTime;
        private int _currentExplosionCont; 

        private bool _play;
        private bool _childDestroyd;

        private Animator _animator;
        private static readonly int AnimatorDestroy = Animator.StringToHash("Destroy");

        public void Play() => _play = true;

        private void Awake()
        {
            _currentExplosionCont = _explosionCount;

            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_play)
            {
                if (_animator != null)
                {
                    _animator.SetBool(AnimatorDestroy, true);
                }

                if (!_childDestroyd)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                    
                    _childDestroyd = true;
                }
                
                float currentTime = Time.time;
                
                if (_currentExplosionCont > 0)
                {
                    if (currentTime - _lastExplosionTime > _explosionDelay)
                    {
                        _lastExplosionTime = currentTime;
                        _currentExplosionCont--;

                        for (int i = 0; i < _batchSize; i++)
                        {
                            GameObject explosionGameObject = Instantiate(_explosionTemplate, transform);
                            
                            explosionGameObject.transform.localPosition = new Vector3(
                                Random.Range(-_destroyRadius, _destroyRadius),
                                Random.Range(-_destroyRadius, _destroyRadius),
                                Random.Range(-_destroyRadius, _destroyRadius)
                            );

                            float scale = Random.Range(_minExplosionRadius, _maxExplosionRadius);
                            
                            explosionGameObject.transform.localScale = new Vector3(scale, scale, scale);
                        }
                    }
                }
                else
                {
                    if (currentTime - _lastExplosionTime > _destroyDelay)
                    {
                        Finished?.Invoke(this, EventArgs.Empty);
                        
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}