using System;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Object
{
    [Serializable]
    public enum ControlType
    {
        Buttons = 0,
        PhoneRotation = 1
    }
    
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ObjectControl : MonoBehaviour
    {
        // Require variable
        [HideInInspector] public float Speed;
        [Header("Effects: ")]
        public BlastWave Explosion;

        // Other variable
        private Rigidbody _rigidbody;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            _rigidbody.velocity = transform.up * Speed;
            
            OnUpdate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bounds"))
            {
                var normal = collision.GetContact(0).normal;
                var reflect = Vector3.Reflect(transform.up, normal);
                var angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg - 90f;
                transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
            
            OnCollision(collision);
        }
        
        protected virtual void OnCollision(Collision collision) { }
        protected virtual void OnUpdate() { }
    }
}
