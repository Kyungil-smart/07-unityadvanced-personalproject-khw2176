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

    public float CurrentHP { get; private set; }
    public float CurrentDamage { get; private set; }

    public event Action OnDeath;
    public event Action OnStatChanged;

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
        OnStatChanged?.Invoke();

        if (CurrentHP <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void AddDamage(float value)
    {
        CurrentDamage += value;
        OnStatChanged?.Invoke();
    }

    public void Heal(float value)
    {
        CurrentHP += value;
        if (CurrentHP > maxHP)
            CurrentHP = maxHP;

        OnStatChanged?.Invoke();
    }

    public void ResetStat()
    {
        CurrentHP = maxHP;
        CurrentDamage = baseDamage;
        OnStatChanged?.Invoke();
    }
}