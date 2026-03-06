using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] Transform target;

    [Header("카메라 위치")]
    [SerializeField] Vector3 offset = new Vector3(0, 20, -15); // Y, Z값 늘려 넓게 보기

    [Header("추적 속도")]
    [SerializeField] float smoothTime = 0.3f; // 조금 느리게 부드럽게

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );

        transform.LookAt(target);
    }
}