using UnityEngine;

public enum ItemType { Heal, AttackUp }

public class Item : MonoBehaviour
{
    public ItemType type;
    public float amount; // Heal량 또는 AttackUp량

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStat>(out PlayerStat player))
        {
            if (type == ItemType.Heal)
                player.Heal(amount);
            else if (type == ItemType.AttackUp)
                player.AddDamage(amount);

            Destroy(gameObject);
        }
    }
}