using UnityEngine;

public class EnemyStat : MonoBehaviour, IDamageable
{
    [SerializeField] int stage = 1;
    [SerializeField] GameObject explosionPrefab;

    [Header("아이템 설정")]
    [SerializeField] GameObject healItemPrefab;
    [SerializeField] GameObject attackItemPrefab;

    [SerializeField] float healAmount = 4f;
    [SerializeField] float attackAmount = 0.5f;


    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public float Damage { get; private set; }

    void Start()
    {
        var data = EnemyStatLoader.Load(stage);

        MaxHP = data.hp;
        CurrentHP = MaxHP;
        Damage = data.damage;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position + Vector3.up * 0.5f,
                Quaternion.identity
            );
        }

        SpawnItem();

        GameManager.Instance.EnemyKilled();

        Destroy(gameObject);
    }

    void SpawnItem()
    {
        if (healItemPrefab == null || attackItemPrefab == null)
            return;

        // 스테이지별 아이템 확률 / 개수
        // 1스테이지: 공격 2개, 체력 3개
        // 2스테이지: 공격 3개, 체력 4개
        // 3스테이지: 공격 4개, 체력 5개
        // 적 1마리 죽일 때 50% 확률로 아이템 생성 예시

        float rand = Random.value;

        // 공격력 아이템
        if (rand < 0.5f)
        {
            GameObject item = Instantiate(attackItemPrefab, transform.position, Quaternion.identity);
            ItemPickup pickup = item.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.type = ItemType.AttackUp;
                pickup.attackAmount = attackAmount;
            }
        }
        else // 체력 아이템
        {
            GameObject item = Instantiate(healItemPrefab, transform.position, Quaternion.identity);
            ItemPickup pickup = item.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.type = ItemType.Heal;
                pickup.healAmount = healAmount;
            }
        }
    }
}