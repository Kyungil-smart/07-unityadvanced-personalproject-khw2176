using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Image fillImage;

    EnemyStat stat;
    float maxHP;

    void Start()
    {
        stat = GetComponentInParent<EnemyStat>();
        maxHP = stat.MaxHP;
    }

    void Update()
    {
        if (stat == null) return;

        fillImage.fillAmount = stat.CurrentHP / stat.MaxHP;

        transform.forward = Camera.main.transform.forward;
    }
}