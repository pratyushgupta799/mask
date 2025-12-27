using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletForwardForce = 20f;
    [SerializeField] private float bulletUpwardForce = 8f;
    
    [Header("States")]
    [SerializeField] private float attackRange;
    public bool playerInAttackRange;
    
    [Header("Health")]
    [SerializeField] private int health = 100;
    
    // Attacking
    bool alreadyAttacked;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (playerInAttackRange)
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }

    private void AttackPlayer()
    {
        agent.ResetPath();
        
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            
            rb.AddForce(transform.forward * bulletForwardForce, ForceMode.Impulse);
            rb.AddForce(transform.up * bulletUpwardForce, ForceMode.Impulse);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
