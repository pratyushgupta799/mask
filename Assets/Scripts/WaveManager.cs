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

    private bool waveSpawning;

    private int waveIndex = 0;
    private readonly HashSet<Enemy> aliveEnemies = new();

    private bool waveQueued;

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
        EnemyUI.text = "Alive: " + aliveEnemies.Count;
    }
    
    private void StartNextWave()
    {
        waveQueued = false;
        waveIndex++;
        if (waveIndex > GameManager.Instance.highestWave)
        {
            GameManager.Instance.highestWave = waveIndex;
        }
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        waveSpawning = true;
        
        WaveUI.text = "Wave " + waveIndex;
        EnemyUI.text = "Alive: " + aliveEnemies.Count;
        int budget = waveSettings.baseBudget + waveIndex * waveSettings.budgetGrowth;

        while (budget > 0)
        {
            if (aliveEnemies.Count >= waveSettings.maxAlive)
            {
                yield return null;
                continue;
            }

            EnemyEntry entry = PickEnemy(budget);
            if (entry == null) break;

            Spawn(entry);
            budget -= entry.cost;
            
            yield return new WaitForSeconds(waveSettings.spawnDelay);
        }
        
        waveSpawning = false;
    }

    private void Spawn(EnemyEntry entry)
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var enemy = Instantiate(entry.prefab, sp.position, sp.rotation).GetComponent<Enemy>();
        aliveEnemies.Add(enemy);
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        aliveEnemies.Remove(enemy);
        totalKills++;

        if (aliveEnemies.Count == 0 && !waveSpawning && !waveQueued)
        {
            waveQueued = true;
            Invoke(nameof(StartNextWave), 3f);
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
