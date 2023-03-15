using System.Collections;
using Assets.Scripts.Control;
using Assets.Scripts.Enemy;
using Assets.Scripts.Object;
using Assets.Scripts.Shoot;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Scripts.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Objects/New Player Data", order = 1)]
    public class Player : ScriptableObject
    {
        #region Main

        [Header("Behavior: "), Space(10)]
        public GameObject PlayerPrefab;

        #endregion
        
        #region Settings
        
        [Header("Settings: "), Space(10)]
        [Range(1, 2)] public int Damage;
        [Range(0, 1)] public float FireRate;
        
        #endregion
        
        #region Movement

        [Header("Movement: "), Space(10)]
        public float Speed;
        public float RotationSpeed;
        public ControlType ControlType;
        
        #endregion
        
        #region Attacks

        [Header("Other: "), Space(10)]
        public Bullet Bullet;
        public GameObject Morgenstern;

        #endregion
        
        #region Visual

        private void SetStartSettings(PlayerMovement _playerControl)
        {
            _playerControl.Speed = Speed;
            _playerControl.RotateSpeed = RotationSpeed;
            _playerControl.ControlType = ControlType;
            _playerControl.enabled = true;
        }

        private void SetStartShootingSettings(Shooting _shooting)
        {
            _shooting.Bullet = Bullet;
            _shooting.fireRate = FireRate;
        }
        
        public void Instantiate(Transform _spawnPoint, out GameObject _currentPlayer)
        {
            // Spawn
            _currentPlayer = Instantiate(PlayerPrefab, _spawnPoint);
            var _player = _currentPlayer.transform;
            _player.localPosition = Vector3.zero;
            _player.localRotation = quaternion.identity;

            SetStartShootingSettings(_player.GetComponent<Shooting>());
            SetStartSettings(_player.GetComponent<PlayerMovement>());
        }

        #endregion
    }
}
