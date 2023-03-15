using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.Class
{
    [Serializable]
    public class GameData
    {
        public int Score = 0; // Score
        public int Combo = 0; // Combo
        public int ScoreCurrentWave = 0; // What enemy you destroy from current wave
        public int IndexCurrentWave = 1; // Your wave
        public int GetTotalIndexOfWave => IndexCurrentWave * 5; // Return what enemy you should to destroy in this s wave
        public float GetPercent => (float)ScoreCurrentWave / GetTotalIndexOfWave; // Get percent between current score and total 

        // Strings for UI
        public string StringCombo => $"{Combo:00}"; // Return how should to show Combo {01}
        public string StringWave => $"{ScoreCurrentWave}/{GetTotalIndexOfWave}"; // Return how should to show Wave {1/15}
        public string StringScore => Score.ToString("N0", new CultureInfo("de-DE", false).NumberFormat); // Return how should to show Score {1.123.456}
    }
    
    [Serializable]
    public enum EnemyType
    {
        Simple,
        Laser,
        Morgenshtern,
        Follow,
        Explosion,
    }

    [Serializable]
    public enum WhereCameraWatch
    {
        Root,
        Start,
        Settings,
        Shop,
        Record,
        Pause
    }
}
