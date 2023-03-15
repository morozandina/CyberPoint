using System;
using UnityEngine;

namespace Assets.Scripts.Shoot
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Bullet : MonoBehaviour
    {
        [Header("Settings:")]
        public float BulletSpeed;
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rigidbody.velocity = transform.up * BulletSpeed;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Bounds"))
                return;
            
            Destroy(gameObject);
        }
    }
}
