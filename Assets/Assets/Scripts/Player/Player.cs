using System.Collections;
using Assets.Scripts.Control;
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
        
        private static void SetStartSettingsForVisual(VisualEffect _materialize, GameObject _player)
        {
            var mesh = _player.GetComponent<MeshFilter>().sharedMesh;
            var meshMaterial = _player.GetComponent<MeshRenderer>().material;
            
            _materialize.SetMesh("MeshToMaterialize", mesh);
            _materialize.SetVector4("Color", meshMaterial.GetColor("_EmissionColor"));
            _materialize.SetVector4("EmissionColor", meshMaterial.GetColor("_EmissionColor"));
        }
        
        public void Instantiate(Transform _spawnPoint, out GameObject _currentPlayer)
        {
            // Spawn
            _currentPlayer = Instantiate(PlayerPrefab, _spawnPoint);
            _currentPlayer.GetComponent<PlayerMovement>().Speed = 0;
            _currentPlayer.GetComponent<Shooting>().enabled = false;
        }
        
        public IEnumerator Instantiate(Transform player)
        {
            player.localPosition = Vector3.zero;
            player.localRotation = quaternion.identity;
            
            var materialize = player.GetComponent<VisualEffect>();
            
            SetStartSettingsForVisual(materialize, player.GetChild(0).gameObject);
            // Execute animation for visual
            materialize.enabled = true;
            materialize.Play();
            
            yield return new WaitForSeconds(2f);
            player.GetComponent<Shooting>().enabled = true;
            SetStartShootingSettings(player.GetComponent<Shooting>());
            yield return new WaitForSeconds(1f);
            
            materialize.enabled = false;
            
            var coll = player.GetComponent<SphereCollider>();
            coll.enabled = true;
            
            player.GetChild(0).gameObject.SetActive(true);
            player.GetChild(1).gameObject.SetActive(true);
            
            SetStartSettings(player.GetComponent<PlayerMovement>());
            yield return null;
        }
        
        #endregion
    }
}
