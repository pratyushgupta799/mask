using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAI : Enemy
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
    
    private Floating floating;
    
    // Attacking
    bool alreadyAttacked;

    private bool playerInAttackRangeThisFrame;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        floating = GetComponent<Floating>();
    }

    private void Update()
    {
        Physics.CheckSphere(transform.position + Vector3.up, attackRange, playerLayer);
        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackRange)
        {
            if (playerInAttackRange != true)
            {
                playerInAttackRangeThisFrame = true;
            }
            else
            {
                playerInAttackRangeThisFrame = false;
            }

            playerInAttackRange = true;
        }
        else
        {
            playerInAttackRange = false;
            playerInAttackRangeThisFrame = false;
        }
        
        if (playerInAttackRange)
        {
            floating.SetFloatable(false);
            AttackPlayer();
        }
        else
        {
            floating.SetFloatable(true);
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.SetDestination(target.position);
        // Debug.Log("Chasing");
    }

    private void AttackPlayer()
    {
        // Debug.Log("Attacking");
        agent.isStopped = true;
        agent.updatePosition = false; 
        agent.velocity = Vector3.zero;
        
        transform.LookAt(target);

        if (playerInAttackRangeThisFrame)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
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
