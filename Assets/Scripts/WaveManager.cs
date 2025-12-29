using System.Collections;
using System.Collections.Generic;
using GlobalData;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave")]
    [SerializeField] private WaveSettings waveSettings;
    [SerializeField] private List<EnemyEntry> enemies;

    [Header("Spawning")] 
    [SerializeField] private Transform[] spawnPoints;

    [Header("References")] 
    [SerializeField] private Text WaveUI;
    [SerializeField] private Text EnemyUI;

    private int waveIndex = 0;
    private int aliveEnemies = 0;

    private int totalKills = 0;

    void OnEnable()
    {
        Enemy.OnEnemyKilled += OnEnemyKilled;
    }
    
    void OnDisable()
    {
        Enemy.OnEnemyKilled -= OnEnemyKilled;
    }
    
    private void Start()
    {
        StartNextWave();
    }

    private void Update()
    {
        EnemyUI.text = "Alive: " + aliveEnemies;
    }
    
    private void StartNextWave()
    {
        waveIndex++;
        if (waveIndex > GameManager.Instance.highestWave)
        {
            GameManager.Instance.highestWave = waveIndex;
        }
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        WaveUI.text = "Wave " + waveIndex;
        EnemyUI.text = "Alive: " + aliveEnemies;
        int budget = waveSettings.baseBudget + waveIndex * waveSettings.budgetGrowth;

        while (budget > 0)
        {
            if (aliveEnemies >= waveSettings.maxAlive)
            {
                yield return null;
                continue;
            }

            EnemyEntry entry = PickEnemy(budget);
            if (entry == null) break;

            Spawn(entry);
            budget -= entry.cost;
            aliveEnemies++;
            
            yield return new WaitForSeconds(waveSettings.spawnDelay);
        }
    }

    private void Spawn(EnemyEntry entry)
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(entry.prefab, sp.position, sp.rotation);
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        aliveEnemies--;
        totalKills++;
        if (aliveEnemies <= 0)
        {
            StartNextWave();
        }
    }

    EnemyEntry PickEnemy(int budget)
    {
        List<EnemyEntry> valid = enemies.FindAll(e => e.cost <= budget);
        if (valid.Count == 0) return null;
        
        float totalWeight = 0f;
        foreach (var e in valid)
        {
            totalWeight += e.weight;
        }
        
        float roll = Random.value * totalWeight;
        foreach (var e in valid)
        {
            roll -= e.weight;
            if (roll <= 0f)
            {
                return e;
            }
        }

        return valid[0];
    }
    
    public int GetTotalKills()
    {
        return totalKills;
    }

    public int GetCurrentWave()
    {
        return waveIndex;
    }
}
