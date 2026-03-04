using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;

    [SerializeField] float lifeTime = 3f;

    public void SetDamage(float value)
    {
        damage = value;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}