using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Text waveText;
    [SerializeField] private Text killText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private WaveManager waveManager;
    
    private void OnEnable()
    {
        waveText.text = "Waves: " + waveManager.GetCurrentWave().ToString();
        killText.text = "Kills: " + waveManager.GetTotalKills().ToString();
        highScoreText.text = "Highest Wave: " + PlayerPrefs.GetInt("waveHighScore", 0).ToString();
    }
}
