using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("감지 설정")]
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float fireRange = 8f;

    [Header("포신")]
    [SerializeField] private Transform turret;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    private NavMeshAgent agent;
    private Transform player;
    private EnemyStat stat;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stat = GetComponent<EnemyStat>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            agent.SetDestination(player.position);

            RotateTurret();

            if (dist <= fireRange)
            {
                Fire();
            }
        }
    }

    void RotateTurret()
    {
        Vector3 dir = player.position - turret.position;
        dir.y = 0;
        turret.rotation = Quaternion.LookRotation(dir);
    }

    void Fire()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}