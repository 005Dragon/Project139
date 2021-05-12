using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class GunController : MonoBehaviour
    {
        public class ShotFinishedEventArgs : EventArgs
        {
            public ShotModel Shot { get; }

            public ShotFinishedEventArgs(ShotModel shot)
            {
                Shot = shot;
            }
        }
        
        public event EventHandler<ShotFinishedEventArgs> ShotFinished;
        
        public GameObject BeamTemplate;

        public float SpeedRotation = 2f;
        public float TargetLockDiff = 1f;
        public bool Inverse;

        public bool ActionInProcess => _currentShot != null || _shotQueue.Count > 0 || _countShotsInProgress != 0;
        
        private Animator _gunAnimator;
        private AnimationVisibleController _laserBeamAnimationVisibleController;

        private static readonly int AnimatorShot = Animator.StringToHash("Shot");

        private ShotModel _currentShot;
        private int _countShotsInProgress;
        private readonly Queue<ShotModel> _shotQueue = new Queue<ShotModel>();
        
        public void SimpleShot(Transform target, bool hit, float damage)
        {
            const int shotsCount = 3;
            
            for (int i = 0; i < shotsCount; i++)
            {
                var shotModel = new ShotModel(target, 2.0f, ShotModel.SpeedType.Fast, damage / shotsCount)
                {
                    Explosion = hit,
                    ExplosionScale = 1 + 0.2f * i
                };
                
                _shotQueue.Enqueue(shotModel);
            }
        }

        public void DirectionShot(Transform target, bool hit, float damage)
        {
            var shotModel = new ShotModel(target, 6.0f, ShotModel.SpeedType.Slow, damage)
            {
                Explosion = hit,
                ExplosionScale = 2.0f
            };

            _shotQueue.Enqueue(shotModel);
        }

        private void Awake()
        {
            _gunAnimator = GetComponent<Animator>();
            _laserBeamAnimationVisibleController = GetComponentInChildren<AnimationVisibleController>();
        }

        private void Update()
        {
            if (_currentShot == null && _shotQueue.Count > 0)
            {
                _currentShot = _shotQueue.Dequeue();
            }

            if (_currentShot == null)
            {
                RotateToAngle(0);
            }
            else
            {
                if (!_currentShot.TargetLocked)
                {
                    _currentShot.TargetLocked = RotateToTarget(_currentShot.Target);
                }
                else if (!_currentShot.Charged && !_currentShot.Charging)
                {
                    Charging();
                }
                else if (_currentShot.Charged)
                {
                    Shot(_currentShot);
                    _currentShot = null;
                }
            }
        }

        private void Charging()
        {
            _currentShot.Charging = true;
            _gunAnimator.SetBool(AnimatorShot, true);
        }

        private void Charged()
        {
            _currentShot.Charging = false;
            _currentShot.Charged = true;
            _gunAnimator.SetBool(AnimatorShot, false);
        }

        private void Shot(ShotModel shotModel)
        {
            _currentShot.Charged = false;

            _laserBeamAnimationVisibleController.Show();

            GameObject beam = Instantiate(BeamTemplate);

            var conditionalParent = _laserBeamAnimationVisibleController.transform;
            
            beam.transform.position = conditionalParent.position;
            beam.transform.rotation = conditionalParent.rotation;

            var beamController = beam.GetComponent<BeamController>();

            beamController.ShotModel = shotModel;
            beamController.Finished += OnBeamControllerFinished;
            
            beamController.Initialize();

            _countShotsInProgress++;
        }

        private void OnBeamControllerFinished(object sender, EventArgs e)
        {
            var beamController = (BeamController) sender;

            beamController.Finished -= OnBeamControllerFinished;
            
            ShotFinished?.Invoke(this, new ShotFinishedEventArgs(beamController.ShotModel));

            _countShotsInProgress--;
        }

        private bool RotateToTarget(Transform target)
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = target.position;
            
            Vector3 relatedPosition = Inverse ? position - targetPosition : targetPosition - position;

            float angle = Get360DegAngle(Mathf.Atan2(relatedPosition.y, relatedPosition.x) * Mathf.Rad2Deg);

            return RotateToAngle(angle);
        }

        private bool RotateToAngle(float angle)
        {
            var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float currentAngle = Get360DegAngle(transform.localRotation.eulerAngles.z);

            if (Mathf.Abs(currentAngle - angle) < TargetLockDiff)
            {
                transform.localRotation = targetRotation;
                
                return true;
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * SpeedRotation);

            return false;
        }

        private float Get360DegAngle(float angle)
        {
            return (angle + 360) % 360;
        }
    }
}
