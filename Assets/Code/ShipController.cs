using System;
using UnityEngine;

namespace Code
{
    public class ShipController : MonoBehaviour
    {
        public event EventHandler ShipDestroy;

        public event EventHandler HealthChange;

        public event EventHandler EnergyChange;

        public event EventHandler<GunController.ShotFinishedEventArgs> ShotFinished;
        
        public bool ActionInProcess => _gunController.ActionInProcess || !_zoneTaken;
        
        public bool Destroyed { get; private set; }

        public float Heath => _health;

        public float Energy => _energy;
        
        
        public PlayerId Player;
        
        public float MaxHealth;
        
        public float MaxEnergy;
        
        [SerializeField]
        private float _changeZoneSpeed = 2f;
    
        [SerializeField] 
        private BattleZoneDescription battleZoneDescription;

        private DestroyController _destroyController;
        private GunController _gunController;

        private bool _zoneTaken;

        private float _health;
        private float _energy;

        public void TakeDamage(float damageValue)
        {
            _health -= damageValue;

            if (_health <= 0)
            {
                _health = 0;
                
                Destroy();
            }
            
            HealthChange?.Invoke(this, EventArgs.Empty);
        }

        public void SetEnergy(float value)
        {
            _energy = value;
            
            EnergyChange?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeBattleZone(Direction4 direction)
        {
            if (battleZoneDescription.TryGetBattleZoneByDirection(transform.parent, direction, out var resultBattleZone))
            {
                _zoneTaken = false;
                transform.parent = resultBattleZone;
            }
        }

        public void SimpleShot(Transform target, bool hit, float damage) => _gunController.SimpleShot(target, hit, damage);
        
        public void DirectionShot(Transform target, bool hit, float damage) => _gunController.DirectionShot(target, hit, damage);

        public void Destroy() => _destroyController.Play();
        
        private void Awake()
        {
            transform.parent = battleZoneDescription.GetCurrentBattleZone(transform.position);

            _destroyController = GetComponentInChildren<DestroyController>();
            _destroyController.Finished += OnDestroyControllerFinished;
            
            _gunController = GetComponentInChildren<GunController>();
            _gunController.ShotFinished += OnGunControllerShotFinished;

            _health = MaxHealth;
            _energy = MaxEnergy;
        }

        private void OnGunControllerShotFinished(object sender, GunController.ShotFinishedEventArgs eventArgs)
        {
            ShotFinished?.Invoke(this, eventArgs);
        }

        private void OnDestroyControllerFinished(object sender, EventArgs e)
        {
            Destroyed = true;
            
            ShipDestroy?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            if (transform.localPosition.magnitude < 0.1f)
            {
                _zoneTaken = true;
            }

            if (!_zoneTaken)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * _changeZoneSpeed);
            }
        }
    }
}
