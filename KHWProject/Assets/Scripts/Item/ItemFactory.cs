using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance;

    [Header("아이템 프리팹")]
    [SerializeField] private GameObject damageItemPrefab;
    [SerializeField] private GameObject healItemPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnDamage(Vector3 pos)
    {
        Instantiate(damageItemPrefab, pos, Quaternion.identity);
    }

    public void SpawnHeal(Vector3 pos)
    {
        Instantiate(healItemPrefab, pos, Quaternion.identity);
    }
}