using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 3f;

    float damage;
    GameObject owner;   // 🔥 발사한 Enemy

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetOwner(GameObject obj)
    {
        owner = obj;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject hitRoot = other.transform.root.gameObject;

        if (hitRoot == owner) // 자기 자신이면 무시
            return;

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}