using System;
using Assets.Scripts.Enemy;
using Assets.Scripts.Object;
using Assets.Scripts.Player;
using Shaders.RippleDistortion;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public class PlayerMovement : ObjectControl
    {
        [HideInInspector] public float RotateSpeed;
        [HideInInspector] public ControlType ControlType;

        protected override void OnUpdate()
        {
            switch (ControlType)
            {
                // Rotate control
                case ControlType.Buttons:
                    // Rotate the object about the Z axis from Slider value direction
                    transform.Rotate(0, 0, Time.deltaTime * (RotateSpeed * 100) * ButtonsControl.Rotation / 2);
                    break;
                case ControlType.PhoneRotation:
                    // Rotate the object about the Z axis by phone rotation
                    transform.Rotate(0, 0, Time.deltaTime * (RotateSpeed * 100) * -Input.acceleration.x);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                RipplePostProcessor.ins.Ripple(transform.position);
                
                var damage = 1;
                if (other.GetComponent<EnemyControl>())
                    damage = other.GetComponent<EnemyControl>().Damage;
                
                Health.Instance.DamageHp(Explosion, transform, damage);
            }
        }
    }
}
