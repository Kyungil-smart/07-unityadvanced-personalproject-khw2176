using UnityEngine;

public class DamageItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStat.Instance.AddDamage(0.5f);
            Destroy(gameObject);
        }
    }
}