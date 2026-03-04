using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("투사체 설정")]
    [Tooltip("이동 속도")]
    [SerializeField] private float speed = 15f;

    [Tooltip("자동 삭제 시간")]
    [SerializeField] private float lifeTime = 3f;

    private float damage;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (PlayerStat.Instance != null)
            damage = PlayerStat.Instance.CurrentDamage;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            return;

        // Enemy 또는 데미지 받을 수 있는 대상이면 데미지
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(damage);
        }

        // Enemy, Obstacle, Default 등에 맞으면 삭제
        Destroy(gameObject);
    }
}