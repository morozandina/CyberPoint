using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Class;
using Assets.Scripts.Enemy;
using Assets.Scripts.Shoot;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static Action EnemyDestroyed;

        [Header("In game components:")]
        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] private TextMeshPro[] comboText;
        [SerializeField] private TextMeshPro[] waveText;
        [SerializeField] private SpriteRenderer waveIndicator;

        [Space(15)] [Header("Player")]
        public Player.Player Player;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform bulletParent;
        private GameObject _currentPlayer;
        
        [Space(15)] [Header("Enemy // AI")]
        [SerializeField] private Transform spawnPoint;
        public List<Enemy.Enemy> Enemies;
        [SerializeField] private Vector3 spawnMinPosition;
        [SerializeField] private Vector3 spawnMaxPosition;
        private Enemy.Enemy _getEnemies => Enemies[Random.Range(0, Enemies.Count)];
        private Quaternion _getRotation => Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 180)));
        private Vector3 _currentSpawnedPosition;

        [Space(15)] [Header("Other")]
        public Material ComboMaterial;
        [SerializeField] private float comboTime;
        [SerializeField] private int maxEnemyInGame;
        [SerializeField] private float spawnTimeBetween;
        
        // Wave Control
        private bool isNewWave;
        private bool isKilled;
        private int _currentEnemyInGame;
        
        private int getEnemyCount => spawnPoint.childCount;
        private static readonly int Fade = Shader.PropertyToID("_Fade");

        // Current game data
        private GameData _gameData;

        private void OnEnable()
        {
            EnemyDestroyed += UpdateGameData;
            ComboMaterial.SetFloat(Fade, 0);
            
            _gameData = new GameData();
            isNewWave = true;
            DateToUI();
            
            SpawnPlayer();
            StartCoroutine(StartSpawning());
        }

        private void OnDisable() => EnemyDestroyed -= UpdateGameData;
        // UI: Increase score
        private void IncreaseScore() => _gameData.Score += 10 * _gameData.Combo;
        
        // UI: Increase Combo
        private void IncreaseCombo()
        {
            if (_gameData.Combo < 50)
                _gameData.Combo++;
            
            if (_comboTimer != null)
                StopCoroutine(_comboTimer);
            _comboTimer = StartCoroutine(ComboTimer());
        }
        // UI: Control Combo Timer
        private Coroutine _comboTimer;
        private IEnumerator ComboTimer() // Time for combo
        {
            var totalTime = 0f;
            ComboMaterial.SetFloat(Fade, 1);

            while (totalTime <= comboTime)
            {
                ComboMaterial.SetFloat(Fade, 1 - totalTime / comboTime);
                totalTime += Time.deltaTime;
                yield return null;
            }

            _gameData.Combo = 0;
            foreach (var c_text in comboText)
                c_text.text = _gameData.StringCombo;
        }
        // UI: Increase Wave
        private void IncreaseWave()
        {
            _gameData.ScoreCurrentWave += 1;

            if (_gameData.ScoreCurrentWave <= _gameData.GetTotalIndexOfWave)
                return;

            isNewWave = false;
            _gameData.ScoreCurrentWave = 0;
            _gameData.IndexCurrentWave += 1;

            if (_gameData.IndexCurrentWave % 3 == 0)
                maxEnemyInGame++;

            // Do something if wave is complete ...
            EnemyControl.DestroyAll?.Invoke();
            
        }
        // UI: Change UI
        private void DateToUI()
        {
            scoreText.text = _gameData.StringScore;
            foreach (var c_text in comboText)
                c_text.text = _gameData.StringCombo;
            foreach (var w_text in waveText)
                w_text.text = _gameData.StringWave;
            waveIndicator.size = new Vector2(_gameData.GetPercent, 1);
        }

        private void UpdateGameData()
        {
            IncreaseCombo();
            IncreaseScore();
            IncreaseWave();
            
            DateToUI();

            isKilled = true;
        }

        // Spawn Player
        private void SpawnPlayer()
        {
            Player.Instantiate(playerSpawnPoint, out _currentPlayer);
            _currentPlayer.GetComponent<Shooting>().bulletsParent = bulletParent;
        }

        // Spawn Enemies
        private void SpawnEnemies()
        {
            _currentSpawnedPosition.Random(spawnMinPosition, spawnMaxPosition);
            StartCoroutine(_getEnemies.Instantiate(spawnPoint, _currentSpawnedPosition, _getRotation, _currentPlayer.GetComponent<SphereCollider>()));
        }

        private IEnumerator StartSpawning()
        {
            _currentEnemyInGame = maxEnemyInGame;
            
            while (getEnemyCount < _currentEnemyInGame)
            {
                SpawnEnemies();
                
                yield return new WaitForSeconds(spawnTimeBetween);
            }

            StartCoroutine(AfterSpawning());
        }

        private IEnumerator AfterSpawning()
        {
            while (isNewWave)
            {
                yield return new WaitUntil(() => isKilled);
                
                isKilled = false;
                SpawnEnemies();
            }
        }

        private void StartNewWave()
        {
            isNewWave = true;
            StartCoroutine(StartSpawning());
        }
    }

    [Serializable]
    public static class Utils
    {
        public static void Random(this ref Vector3 _this, Vector3 _min, Vector3 _max)
        {
            _this = new Vector3(UnityEngine.Random.Range(_min.x, _max.x), UnityEngine.Random.Range(_min.y, _max.y), UnityEngine.Random.Range(_min.z, _max.z));
        }

        public static void Ignore(this Collider _this, Collider withWho)
        {
            Physics.IgnoreCollision(withWho, _this);
        }
    }
}
