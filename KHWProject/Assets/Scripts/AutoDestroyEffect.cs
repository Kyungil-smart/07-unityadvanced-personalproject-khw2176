using UnityEngine;

public class AutoDestroyEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2f); // 2초 후 자동 삭제
    }
}