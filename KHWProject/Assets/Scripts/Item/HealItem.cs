using UnityEngine;

public class HealItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStat.Instance.Heal(4f);
            Destroy(gameObject);
        }
    }
}