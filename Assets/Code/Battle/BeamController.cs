using System;
using Code.Battle.Core;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Battle
{
    public class BeamController : MonoBehaviour, IManagedInitializable
    {
        public event EventHandler Finished;
        
        public ShotModel ShotModel { get; set; }

        [SerializeField]
        private GameObject _explosionTemplate;
        
        private const float _timeBeamScalePercent = 0.1f;
        
        private float _processPercent = 0f;

        private float _beamLifeTime;
        private float _beamStartChangeScale;

        private Vector2 _startPosition;
        private Vector2 _targetPosition;

        public void Initialize()
        {
            _startPosition = transform.position;
            _targetPosition = ((BattleZoneField)ShotModel.Target).Transform.position;
            
            _beamLifeTime = 0f;
        }

        private void Update()
        {
            if (_processPercent > 0.99f)
            {
                if (ShotModel.Explosion && _explosionTemplate != null)
                {
                    ShotModel.Explosion = false;
                    GameObject explosion = Instantiate(_explosionTemplate, _targetPosition, quaternion.identity);

                    explosion.transform.localScale =
                        new Vector3(ShotModel.ExplosionScale, ShotModel.ExplosionScale, ShotModel.ExplosionScale);

                    var animationVisibleController = explosion.GetComponent<AnimationVisibleController>();
                    animationVisibleController.Initialize();
                    animationVisibleController.Show();
                }
                
                Finished?.Invoke(this, EventArgs.Empty);
                
                Destroy(gameObject);
            }
            
            UpdateBeamPosition();

            _processPercent = CalculateProcessPercent();
            
            UpdateBeamScale(_processPercent);
        }

        private void UpdateBeamPosition()
        {
            _beamLifeTime += Time.deltaTime;
            
            float shotTime = ShotModel.Speed == ShotModel.SpeedType.Fast ? 0.45f : 0.75f;

            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _beamLifeTime / shotTime);
        }

        private float CalculateProcessPercent()
        {
            float targetRange = (_targetPosition - _startPosition).magnitude;
            float currentRange = ((Vector2)transform.position - _startPosition).magnitude;
            
            return currentRange / targetRange;
        }

        private void UpdateBeamScale(float processPercent)
        {
            Vector3 currentScale = transform.localScale;
            
            if (processPercent > 0.0f && processPercent < _timeBeamScalePercent)
            {
                transform.localScale = GetUpScaleBeam(currentScale, processPercent);
                
                return;
            }
            
            if (processPercent > (1.0f - _timeBeamScalePercent))
            {
                transform.localScale = GetDownScaleBeam(currentScale, processPercent);
            }
        }

        private Vector3 GetUpScaleBeam(Vector3 currentScale, float processPercent)
        {
            float xScale = Mathf.Lerp(0.0f, ShotModel.Length, processPercent / _timeBeamScalePercent);
            
            return new Vector3(xScale, currentScale.y, currentScale.z);
        }

        private Vector3 GetDownScaleBeam(Vector3 currentScale, float processPercent)
        {
            float percent = processPercent - (1.0f - _timeBeamScalePercent);
            
            float xScale = Mathf.Lerp(ShotModel.Length, 0.0f, percent / _timeBeamScalePercent);

            return new Vector3(xScale, currentScale.y, currentScale.z);
        }
    }
}
