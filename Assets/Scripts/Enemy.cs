using GlobalData;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    public int cost = 1;
    public int health = 100;

    public static System.Action<Enemy> OnEnemyKilled;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        OnEnemyKilled?.Invoke(this);
        Destroy(gameObject);
    }
}
