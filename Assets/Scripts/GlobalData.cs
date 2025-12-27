using UnityEngine;

namespace GlobalData
{
    public enum EnemyType
    {
        Melee,
        Ranged
    }

    [System.Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public GameObject prefab;
        public int cost;
        public float weight;
    }
    
    [System.Serializable]
    public class WaveSettings
    {
        public int baseBudget = 5;
        public int budgetGrowth = 3;
        public int maxAlive = 10;
        public float spawnDelay = 0.5f;
    }
}
