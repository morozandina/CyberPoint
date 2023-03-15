using UnityEngine;

namespace Assets.Scripts.Shoot
{
    public class Shooting : MonoBehaviour
    {
        [HideInInspector] public Bullet Bullet;
        [HideInInspector] public float fireRate = 1f;
        [HideInInspector] public Transform bulletsParent;
        private float _fireNext;

        private void Update()
        {
            if (!(Time.time > _fireNext))
                return;
            
            _fireNext = Time.time + fireRate;
            SpawnBullet(transform);
        }

        private void SpawnBullet(Transform _pos)
        {
            var _bullet = Instantiate(Bullet, bulletsParent).transform;
            _bullet.localPosition = _pos.localPosition;
            _bullet.localRotation = _pos.localRotation;
        }
    }
}
