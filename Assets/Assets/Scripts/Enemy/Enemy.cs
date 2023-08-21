using System.Collections;
using Assets.Scripts.Class;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Scripts.Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Objects/New Enemy Data", order = 1)]
    public class Enemy : ScriptableObject
    {
        #region Main

        [Header("Behavior: "), Space(10)]
        public EnemyControl EnemyPrefab;
        [Range(1, 5)] public int EnemyLevel;
        public EnemyType BehaviourType;

        #endregion
        
        #region Settings
        
        [Header("Settings: "), Space(10)]
        public int Hp;
        public int Damage;
        
        #endregion
        
        #region Movement

        [Header("Movement: "), Space(10)]
        public float Speed;
        public float MoveTime;
        public float WaitTime;

        #endregion
        
        #region Morgenstern
        
        [Header("Other: "), Space(10)]
        public GameObject Morgenstern;

        #endregion
        
        #region Visual

        private static void SetStartSettingsForVisual(VisualEffect _materialize, GameObject _enemy)
        {
            var mesh = _enemy.GetComponent<MeshFilter>().sharedMesh;
            var meshMaterial = _enemy.GetComponent<MeshRenderer>().material;
            
            _materialize.SetMesh("MeshToMaterialize", mesh);
            _materialize.SetVector4("Color", meshMaterial.GetColor("_EmissionColor"));
            _materialize.SetVector4("EmissionColor", meshMaterial.GetColor("_EmissionColor"));
        }

        private void SetStartSettings(EnemyControl _enemyControl)
        {
            var speedAmplifier = (int)(GameManager.Instance.GetGameData.IndexCurrentWave / 3);
            var hpAmplifier = (int)(GameManager.Instance.GetGameData.IndexCurrentWave / 5);
            var damageAmplifier = (int)(GameManager.Instance.GetGameData.IndexCurrentWave / 10); 
            
            _enemyControl.Speed = Speed + speedAmplifier;
            _enemyControl.Damage = Damage + damageAmplifier;
            _enemyControl.Hp = Hp + hpAmplifier;
            
            _enemyControl.enabled = true;
        }
        
        public IEnumerator Instantiate(Transform _spawnPoint, Vector3 _position, Quaternion _rotation, SphereCollider _collider)
        {
            // Spawn
            var enemy = Instantiate(EnemyPrefab, _spawnPoint).transform;
            enemy.localPosition = _position;
            enemy.localRotation = _rotation;

            var materialize = enemy.GetComponent<VisualEffect>();
            
            SetStartSettingsForVisual(materialize, enemy.GetChild(0).gameObject);
            // Execute animation for visual
            materialize.enabled = true;
            materialize.Play();

            yield return new WaitForSeconds(3f);
            if (enemy)
            {
                materialize.enabled = false;

                var coll = enemy.GetComponent<SphereCollider>();
                coll.enabled = true;
                coll.Ignore(_collider);
            
                enemy.GetChild(0).gameObject.SetActive(true);
                SetStartSettings(enemy.GetComponent<EnemyControl>());
            }
            
            yield return null;
        }

        #endregion
    }
}
