using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("AI 설정")]
    [SerializeField] float patrolDistance = 8f;
    [SerializeField] float detectRange = 15f;
    [SerializeField] float attackRange = 7f;
    [SerializeField] float fireRate = 1.5f;

    [Header("터렛 설정")]
    [SerializeField] private Transform turret;
    [SerializeField] private float turretRotateSpeed = 180f;

    [Header("공격")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    [Header("사망 이펙트")]
    [SerializeField] private GameObject explosionPrefab;

    float currentHP;
    float damage;
    float fireTimer;

    NavMeshAgent agent;
    Transform player;
    Vector3 startPos;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
    }

    public void Init(float hp, float dmg)
    {
        currentHP = hp;
        damage = dmg;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            RotateTurretToPlayer();

            if (dist <= attackRange)
            {
                agent.isStopped = true;   // 멈추고 공격
                TryFire();
            }
        }
        else
        {
            Patrol();
        }

        fireTimer -= Time.deltaTime;
    }

    void Patrol()
    {
        if (!agent.hasPath)
        {
            Vector3 randomDir = Random.insideUnitSphere * patrolDistance;
            randomDir += startPos;
            agent.SetDestination(randomDir);
        }
    }

    void RotateTurretToPlayer()
    {
        if (player == null || turret == null) return;

        Vector3 dir = player.position - turret.position;

        dir.y = 0f;   // ⭐ 위아래 회전 제거 (중요)

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        turret.rotation = Quaternion.RotateTowards(
            turret.rotation,
            targetRot,
            turretRotateSpeed * Time.deltaTime
        );
    }

    void TryFire()
    {
        if (fireTimer > 0) return;

        // 터렛이 거의 플레이어 방향일 때만 발사
        Vector3 dirToPlayer = (player.position - turret.position).normalized;
        float angle = Vector3.Angle(turret.forward, dirToPlayer);

        if (angle < 10f)  // 10도 이내면 발사
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<EnemyProjectile>().Init(damage);

            fireTimer = fireRate;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        // 💥 폭발 이펙트 생성
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        StageManager.Instance.EnemyKilled(transform.position);

        Destroy(gameObject);
    }
}