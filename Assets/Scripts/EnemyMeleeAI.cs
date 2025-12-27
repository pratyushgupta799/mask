using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    
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
        Physics.CheckSphere(transform.position, attackRange, playerLayer);
        float dist = Vector3.Distance(transform.position, target.position);
        
        if (dist <= attackRange)
            playerInAttackRange = true;
        else
            playerInAttackRange = false;
        
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
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.SetDestination(target.position);
        Debug.Log("Chasing");
    }

    private void AttackPlayer()
    {
        Debug.Log("Attacking");
        agent.isStopped = true;
        agent.updatePosition = false; 
        agent.velocity = Vector3.zero;
        
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            target.GetComponent<PlayerHealth>().TakeDamage(40);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
