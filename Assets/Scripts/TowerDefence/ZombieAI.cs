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
    private Transform baseTower; // اضافه کردن متغیر برای BaseTower

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        // پیدا کردن BaseTower با تگ
        GameObject baseObj = GameObject.FindGameObjectWithTag("BaseTower");
        if (baseObj != null)
            baseTower = baseObj.transform;
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
        // اگر پلیر و BaseTower هیچکدام نبودند، کاری نکن
        if (player == null && baseTower == null) return;

        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;
        float distanceToBase = baseTower != null ? Vector3.Distance(transform.position, baseTower.position) : Mathf.Infinity;

        isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;
        lastPosition = transform.position;

        // اولویت با پلیر است اگر در محدوده باشد
        if (distanceToPlayer <= playerDetectionRange)
        {
            isFollowingPlayer = true;
        }
        else
        {
            isFollowingPlayer = false;
        }

        if (isFollowingPlayer && player != null)
        {
            FollowPlayer();
        }
        else if (baseTower != null)
        {
            // اگر هنوز به BaseTower نرسیده‌ایم، مسیر pathPoints را دنبال کن
            if (pathPoints != null && currentPathIndex < pathPoints.Length)
            {
                FollowPath();
            }
            else
            {
                FollowBaseTower();
            }
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

    void FollowBaseTower()
    {
        float distanceToBase = Vector3.Distance(transform.position, baseTower.position);
        if (navAgent != null)
        {
            navAgent.SetDestination(baseTower.position);
        }
        else
        {
            if (distanceToBase > attackRange)
            {
                Vector3 direction = (baseTower.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }

        if (distanceToBase <= attackRange &&
            Time.time >= nextAttackTime)
        {
            AttackBaseTower();
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

    void AttackBaseTower()
    {
        isAttacking = true;
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        if (baseTower != null)
        {
            Health baseHealth = baseTower.GetComponent<Health>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(attackDamage);
            }
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
        if (points == null || points.Length == 0)
        {
            Debug.LogWarning($"{name}: مسیر خالی است.");
            return;
        }

        pathPoints = points;
        currentPathIndex = 0;

        if (navAgent != null && pathPoints[0] != null)
        {
            navAgent.SetDestination(pathPoints[0].position);
        }
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