using UnityEngine;
using System;

public class PlayerStat : MonoBehaviour, IDamageable
{
    public static PlayerStat Instance { get; private set; }

    [Header("플레이어 기본 능력치")]
    [Tooltip("기본 체력")]
    [SerializeField] private float maxHP = 12f;

    [Tooltip("기본 공격력")]
    [SerializeField] private float baseDamage = 2.5f;

    [SerializeField] private GameObject explosionPrefab;

    public float CurrentHP { get; private set; }
    public float CurrentDamage { get; private set; }

    public event Action OnStatChanged;
    public event Action OnDeath;

    public void ResetStat()
    {
        CurrentHP = maxHP;
        CurrentDamage = baseDamage;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CurrentHP = maxHP;
        CurrentDamage = baseDamage;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        OnStatChanged?.Invoke();

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void AddDamage(float amount)
    {
        CurrentDamage += amount;
        OnStatChanged?.Invoke();
    }

    public void Heal(float amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, maxHP);

        OnStatChanged?.Invoke();
    }

    void Die()
    {
        // 💥 폭발 이펙트 생성
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject, 0.1f);
    }
}