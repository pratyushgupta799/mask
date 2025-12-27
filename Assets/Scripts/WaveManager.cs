using System.Collections.Generic;
using GlobalData;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave")]
    [SerializeField] private WaveSettings waveSettings;
    [SerializeField] private List<EnemyEntry> enemies;

    [Header("Spawning")] 
    [SerializeField] private Transform[] spawnPoints;

    private int waveIndex = 0;
    private int aliveEnemies = 0;

    void OnEnable()
    {
        // Enemy.OnEnemyKilled += OnEnemyKilled;
    }
    
    void OnDisable()
    {
        // Enemy.OnEnemyKilled -= OnEnemyKilled;
    }
    
    private void Start()
    {
        StartNextWave();
    }
    
    private void StartNextWave()
    {
        waveIndex++;
        // StartCoroutine(SpawnWave());
    }
    
    
}
