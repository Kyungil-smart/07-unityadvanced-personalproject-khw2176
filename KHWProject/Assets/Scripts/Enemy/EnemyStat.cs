using UnityEngine;
using System;

public class EnemyStat : MonoBehaviour, IDamageable
{
    [SerializeField] private int stage = 1;
    [SerializeField] private GameObject explosionPrefab;

    public float CurrentHP { get; private set; }
    public float Damage { get; private set; }

    public event Action OnDeath;

    void Start()
    {
        var stat = EnemyStatLoader.LoadStat(stage);
        CurrentHP = stat.hp;
        Damage = stat.damage;
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
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}