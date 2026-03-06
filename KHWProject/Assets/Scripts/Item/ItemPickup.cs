using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemType type;
    public float healAmount = 4f;
    public float attackAmount = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (PlayerStat.Instance == null)
            return;

        switch (type)
        {
            case ItemType.Heal:
                PlayerStat.Instance.Heal(healAmount);
                break;
            case ItemType.AttackUp:
                PlayerStat.Instance.AddDamage(attackAmount);
                break;
        }

        // 아이템 먹으면 삭제
        Destroy(gameObject);
    }
}