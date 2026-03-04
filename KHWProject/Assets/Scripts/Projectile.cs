using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("투사체 설정")]
    [Tooltip("투사체 이동 속도")]
    [SerializeField] private float speed = 15f;

    [Tooltip("자동 삭제 시간")]
    [SerializeField] private float lifeTime = 3f;

    private float damage;

    private void Start()
    {
        // 플레이어가 발사한 경우
        if (PlayerStat.Instance != null)
            damage = PlayerStat.Instance.CurrentDamage;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // 💥 모든 충돌에서 삭제
    private void OnCollisionEnter(Collision collision)
    {
        // IDamageable이면 데미지 주기
        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable target))
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}