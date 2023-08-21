using System;
using Assets.Scripts.Object;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyControl : ObjectControl
    {
        public static Action DestroyAll;

        [HideInInspector] public int Hp, Damage;
        
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
            if (Hp > 1)
                Hp--;
            else
            {
                GameManager.EnemyDestroyed?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}