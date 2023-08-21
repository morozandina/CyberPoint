using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Health : MonoBehaviour
    {
        public static Health Instance;
        [Header("UI")]
        [SerializeField] private List<SpriteRenderer> partition;
        
        // Local var
        private int _startHp = 2;
        private int _currentHp = 0;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void InitializeHp()
        {
            EnableAllHp();
            _currentHp = _startHp;
        }

        public void IncreaseHp()
        {
            _startHp++;
            _currentHp = _startHp;
            EnableAllHp();
        }

        public void DamageHp(BlastWave explosion, Transform player, int damage)
        {
            if (_currentHp > 0)
                _currentHp -= damage;
            else
                Death(explosion, player);
            partition[_currentHp].enabled = false;
        }

        public void CureHp()
        {
            if (_currentHp < _startHp)
                _currentHp++;

            EnableHpIndicator();
        }

        private void DisableAllHp()
        {
            foreach (var part in partition)
                part.gameObject.SetActive(false);
        }

        private void EnableAllHp()
        {
            DisableAllHp();
            for (var i = 0; i < _startHp; i++)
                partition[i].gameObject.SetActive(true);
        }

        private void EnableHpIndicator()
        {
            for (var i = 0; i < _currentHp; i++)
                partition[i].enabled = true;
        }

        private async void Death(BlastWave explosion, Transform player)
        {
            Instantiate(explosion, player.position, Quaternion.identity);
            Destroy(player.gameObject);
            await Task.Delay(1000);
            Time.timeScale = 0;
        }
    }
}
