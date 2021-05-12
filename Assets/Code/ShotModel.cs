﻿using UnityEngine;

namespace Code
{
    public class ShotModel
    {
        public enum SpeedType
        {
            Slow,
            Fast
        }
            
        public SpeedType Speed { get; }
            
        public float Length { get; }
            
        public Transform Target { get; }

        public bool Explosion { get; set; }
            
        public float ExplosionScale { get; set; } = 1.0f;
            
        public bool TargetLocked { get; set; }
            
        public bool Charging { get; set; }
            
        public bool Charged { get; set; }
            
        public float Damage { get; set; }
            
        public ShotModel(Transform target, float length, SpeedType speed, float damage)
        {
            Target = target;
            Length = length;
            Speed = speed;
            Damage = damage;
        }
    }
}