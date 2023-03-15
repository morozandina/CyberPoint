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
        [Range(1, 5)] public int Hp;
        [Range(1, 2)] public int Damage;
        
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
            var _mesh = _enemy.GetComponent<MeshFilter>().sharedMesh;
            var _meshMaterial = _enemy.GetComponent<MeshRenderer>().material;
            
            _materialize.SetMesh("MeshToMaterialize", _mesh);
            _materialize.SetVector4("Color", _meshMaterial.GetColor("_EmissionColor"));
            _materialize.SetVector4("EmissionColor", _meshMaterial.GetColor("_EmissionColor"));
        }

        private void SetStartSettings(EnemyControl _enemyControl)
        {
            _enemyControl.Speed = Speed;
            _enemyControl.enabled = true;
        }
        
        public IEnumerator Instantiate(Transform _spawnPoint, Vector3 _position, Quaternion _rotation, SphereCollider _collider)
        {
            // Spawn
            var _enemy = Instantiate(EnemyPrefab, _spawnPoint).transform;
            _enemy.localPosition = _position;
            _enemy.localRotation = _rotation;

            var _materialize = _enemy.GetComponent<VisualEffect>();
            
            SetStartSettingsForVisual(_materialize, _enemy.GetChild(0).gameObject);
            // Execute animation for visual
            _materialize.enabled = true;
            _materialize.Play();

            yield return new WaitForSeconds(3f);
            _materialize.enabled = false;

            var _coll = _enemy.GetComponent<SphereCollider>();
            _coll.enabled = true;
            _coll.Ignore(_collider);
            
            _enemy.GetChild(0).gameObject.SetActive(true);
            SetStartSettings(_enemy.GetComponent<EnemyControl>());
            
            yield return null;
        }

        #endregion
    }
}
