using System;
using Code.Battle;
using Code.Utils;
using UnityEngine;

namespace Code
{
    public class ShipController : MonoBehaviour, IBattleShip
    {
        public event EventHandler ShipDestroy;

        public event EventHandler HealthChange;

        public event EventHandler EnergyChange;

        public event EventHandler<EventArgs<ShotModel>> Shot;
        public event EventHandler ChangeBattleZoneFinished;
        public event EventHandler ShotFinished;

        public PlayerSide PlayerSide => _playerSide;

        public bool Destroyed { get; private set; }

        public float Heath => _health;

        public float Energy => _energy;
        
        public float MaxHealth;
        
        public float MaxEnergy;

        [SerializeField] 
        private PlayerSide _playerSide;
        
        [SerializeField]
        private float _changeZoneSpeed = 2f;
    
        [SerializeField] 
        private BattleZoneDescription battleZoneDescription;

        private DestroyController _destroyController;
        private GunController _gunController;

        private bool _zoneTaken;
        private bool _shotFinihsed;

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

        public bool TryChangeBattleZone(Direction4 direction, out IBattleZoneField resultZoneField)
        {
            _zoneTaken = false;
            
            if (battleZoneDescription.TryGetRelativeBattleZoneFieldByDirection(PlayerSide, direction, out IBattleZoneField battleZoneField))
            {
                transform.parent = ((BattleZoneField)battleZoneField).Transform;

                resultZoneField = battleZoneField;
                
                return true;
            }

            resultZoneField = default;
            
            return false;
        }

        public void SimpleShot(IBattleZoneField target, float damage)
        {
            _shotFinihsed = false;
            
            _gunController.SimpleShot(target, damage);
        }

        public void DirectionShot(IBattleZoneField target, float damage)
        {
            _shotFinihsed = false;
            
            _gunController.DirectionShot(target, damage);
        }

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

        private void OnGunControllerShotFinished(object sender, EventArgs<ShotModel> eventArgs)
        {
            Shot?.Invoke(this, eventArgs);
        }

        private void OnDestroyControllerFinished(object sender, EventArgs e)
        {
            Destroyed = true;
            
            ShipDestroy?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            if (!_zoneTaken)
            {
                if (transform.localPosition.magnitude < 0.1f)
                {
                    ChangeBattleZoneFinished?.Invoke(this, EventArgs.Empty);
                
                    _zoneTaken = true;
                }
                
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * _changeZoneSpeed);
            }

            if (!_shotFinihsed)
            {
                if (!_gunController.ActionInProcess)
                {
                    _shotFinihsed = true;
                    
                    ShotFinished?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
