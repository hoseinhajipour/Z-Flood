using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 1f;
    public float playerDetectionRange = 10f;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    [Header("Animation")]
    public Animator animator;
    private bool isMoving = false;
    private bool isAttacking = false;
    private Vector3 lastPosition;

    private Transform[] pathPoints;
    private int currentPathIndex = 0;
    private Transform player;
    private bool isFollowingPlayer = false;
    private float nextAttackTime;
    private NavMeshAgent navAgent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.speed = moveSpeed;
            navAgent.angularSpeed = rotationSpeed;
            navAgent.stoppingDistance = attackRange;
        }
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;
        lastPosition = transform.position;

        if (distanceToPlayer <= playerDetectionRange)
        {
            isFollowingPlayer = true;
        }
        else
        {
            isFollowingPlayer = false;
        }

        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else if (pathPoints != null && pathPoints.Length > 0)
        {
            FollowPath();
        }
    }

    void FollowPath()
    {
        if (currentPathIndex >= pathPoints.Length) return;

        Transform targetPoint = pathPoints[currentPathIndex];
        if (navAgent != null)
        {
            navAgent.SetDestination(targetPoint.position);
        }
        else
        {
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, targetPoint.position) <= stoppingDistance)
        {
            currentPathIndex++;
        }
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (navAgent != null)
        {
            navAgent.SetDestination(player.position);
        }
        else
        {
            if (distanceToPlayer > attackRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }

        if (distanceToPlayer <= attackRange && 
            Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        isAttacking = true;
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        nextAttackTime = Time.time + attackCooldown;
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    public void SetPath(Transform[] points)
    {
        pathPoints = points;
        currentPathIndex = 0;
    }

    public void SetPlayerDetectionRange(float range)
    {
        playerDetectionRange = range;
    }

    public void TakeDamage(float damage)
    {
        Destroy(gameObject);
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
} 