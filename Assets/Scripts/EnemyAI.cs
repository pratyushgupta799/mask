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
    
    [Header("States")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    
    // Attacking
    bool alreadyAttacked;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void ChasePlayer()
    {
        
    }

    private void AttackPlayer()
    {
        
    }
}
