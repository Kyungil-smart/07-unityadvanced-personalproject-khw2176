using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("플레이어 스탯 UI")]
    [Tooltip("체력과 공격력 텍스트")]
    [SerializeField] private TextMeshProUGUI statText;

    private void Start()
    {
        PlayerStat.Instance.OnStatChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (PlayerStat.Instance != null)
            PlayerStat.Instance.OnStatChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        statText.text =
            $"HP : {PlayerStat.Instance.CurrentHP}\n" +
            $"ATK : {PlayerStat.Instance.CurrentDamage}";
    }
}