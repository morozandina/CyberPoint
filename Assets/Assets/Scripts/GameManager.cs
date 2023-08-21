using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Ability;
using Assets.Scripts.Class;
using Assets.Scripts.Enemy;
using Assets.Scripts.Player;
using Assets.Scripts.Shoot;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static Action EnemyDestroyed;
        public TextMeshPro countDown;

        [Header("In game components:")]
        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] private TextMeshPro[] comboText;
        [SerializeField] private TextMeshPro[] waveText;
        [SerializeField] private SpriteRenderer waveIndicator;

        [Space(15)] [Header("Pause")]
        [SerializeField] private GameObject pause;
        [SerializeField] private GameObject upgrade;

        [Space(15)] [Header("Player")]
        public Player.Player player;
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
        
        [Space(15)] [Header("Wave Control")]
        [SerializeField] private float comboTime;
        [SerializeField] private int maxEnemyInGame;
        [SerializeField] private float spawnTimeBetween;
        public UnityEvent finishWaveEvent;
        
        // Wave Control
        private bool _isGame;
        private int _currentEnemyInGame;
        
        private static readonly int Fade = Shader.PropertyToID("_Fade");

        // Current game data
        private GameData _gameData = null;

        public GameData GetGameData => _gameData;

        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 300;
            
            if (Instance == null)
                Instance = this;
            
            EnemyDestroyed += UpdateGameData;
        }

        public void NewInstanceOfGame()
        {
            if (_gameData != null && _gameData.IndexCurrentWave != 0) return;
            
            ComboMaterial.SetFloat(Fade, 0.005f);
            scoreText.text = "0";
            foreach (var cText in comboText)
                cText.text = "00";
            foreach (var wText in waveText)
                wText.text = "0/5";
            waveIndicator.size = new Vector2(0, 1);
        }

        public void StartGame()
        {
            DOVirtual.Float(3, 0, 3, (x) =>
                {
                    countDown.text = x.ToString("n0");
                })
                .SetUpdate(true)
                .SetEase(Ease.Linear)
                .OnComplete(StartPlay);
        }

        private void StartPlay()
        {
            countDown.transform.parent.gameObject.SetActive(false);
            Health.Instance.InitializeHp();
            Time.timeScale = 1;

            if (_gameData == null || _gameData.IndexCurrentWave == 0)
            {
                _gameData = new GameData();
                
                DateToUI();
                SpawnPlayer();
            }
            _isGame = true;
            
            if (_startSpawning != null)
                StopCoroutine(_startSpawning);
            _startSpawning = StartCoroutine(StartSpawning(2));
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            foreach (Transform child in playerSpawnPoint)
                Destroy(child.gameObject);
            foreach (Transform child in bulletParent)
                Destroy(child.gameObject);
            foreach (Transform child in spawnPoint)
                Destroy(child.gameObject);
            
            if (_comboTimer != null)
                StopCoroutine(_comboTimer);
            if (_startSpawning != null)
                StopCoroutine(_startSpawning);
            
            UpgradeUI.ClearCache?.Invoke();
            _gameData = null;
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
            ComboMaterial.SetFloat(Fade, 0.005f);

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

            _isGame = false;
            _gameData.ScoreCurrentWave = 0;
            _gameData.IndexCurrentWave += 1;

            if (_gameData.IndexCurrentWave % 3 == 0)
                maxEnemyInGame++;

            // Do something if wave is complete ...
            EnemyControl.DestroyAll?.Invoke();
            if (_startSpawning != null)
                StopCoroutine(_startSpawning);
            _startSpawning = null;

            OnFinishWave();
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
            if (!_isGame) return;
            
            IncreaseCombo();
            IncreaseScore();
            IncreaseWave();
            
            DateToUI();
            SpawnEnemies();
        }

        // Spawn Player
        private void SpawnPlayer()
        {
            player.Instantiate(playerSpawnPoint, out _currentPlayer);
            StartCoroutine(player.Instantiate(_currentPlayer.transform));
            _currentPlayer.GetComponent<Shooting>().bulletsParent = bulletParent;
        }

        // Spawn Enemies
        private void SpawnEnemies()
        {
            if (!_isGame) return;
            
            _currentSpawnedPosition.Random(spawnMinPosition, spawnMaxPosition);
            StartCoroutine(_getEnemies.Instantiate(spawnPoint, _currentSpawnedPosition, _getRotation, _currentPlayer.GetComponent<SphereCollider>()));
        }

        private Coroutine _startSpawning;
        private IEnumerator StartSpawning(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            _currentEnemyInGame = maxEnemyInGame;
            for (var i = 0; i < _currentEnemyInGame; i++)
            {
                SpawnEnemies();
                yield return new WaitForSeconds(spawnTimeBetween);   
            }
        }
        
        // Game Control
        public void PauseGame(bool state)
        {
            pause.SetActive(true);
            upgrade.SetActive(false);
            Time.timeScale = state ? 0 : 1;
        }
        
        // Finished
        private async void OnFinishWave()
        {
            await Task.Delay(500);
            pause.SetActive(false);
            upgrade.SetActive(true);
            Time.timeScale = 0;
            finishWaveEvent?.Invoke();
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
