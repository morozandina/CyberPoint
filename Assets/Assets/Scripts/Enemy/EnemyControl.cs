using System;
using Assets.Scripts.Object;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyControl : ObjectControl
    {
        public static Action DestroyAll;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Bullet"))
                return;

            DestroyEnemy();
        }

        private void Awake() => DestroyAll += DestroyEnemy;
        private void OnDestroy() => DestroyAll -= DestroyEnemy;

        private void DestroyEnemy()
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            GameManager.EnemyDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}