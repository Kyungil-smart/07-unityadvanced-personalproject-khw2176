using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("감지")]
    [SerializeField] float patrolRange = 10f;
    [SerializeField] float detectRange = 12f;
    [SerializeField] float fireRange = 8f;
    [SerializeField] float fireCooldown = 1.5f;

    [Header("포신")]
    [SerializeField] Transform turret;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

    NavMeshAgent agent;
    Transform player;
    EnemyStat stat;

    float lastFireTime;
    Vector3 startPos;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stat = GetComponent<EnemyStat>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        agent.updateRotation = false; // 🔥 몸 회전 직접 제어
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            agent.SetDestination(player.position);
            RotateBody();
            RotateTurret();

            if (dist <= fireRange)
                TryFire();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 randomDir = Random.insideUnitSphere * patrolRange;
            randomDir += startPos;

            if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, patrolRange, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        RotateBody();
    }

    void RotateBody()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    void RotateTurret()
    {
        if (player == null || turret == null) return;

        Vector3 dir = player.position - turret.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        turret.rotation = Quaternion.Slerp(
            turret.rotation,
            targetRot,
            Time.deltaTime * 10f
        );
    }

    void TryFire()
    {
        if (Time.time < lastFireTime + fireCooldown) return;

        lastFireTime = Time.time;
        Fire();
    }

    void Fire()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        EnemyProjectile proj = bullet.GetComponent<EnemyProjectile>();

        if (proj != null)
        {
            proj.SetDamage(stat.Damage);
            proj.SetOwner(gameObject);   // 🔥 발사자 전달
        }
    }
}