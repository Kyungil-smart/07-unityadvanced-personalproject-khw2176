using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 180f;

    [Header("포신 설정")]
    [SerializeField] private Transform turret;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireCooldown = 0.4f;
    private float lastFireTime;

    private Rigidbody rb;
    private TankInput input;
    private Vector2 moveInput;

    public event Action OnFire;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // 물리 회전 고정
        input = new TankInput();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Fire.performed += Fire;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Fire.performed -= Fire;
        input.Player.Disable();
    }

    private void Update()
    {
        RotateTurret();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void Move()
    {
        // 이동 방향
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        if (dir.sqrMagnitude < 0.01f) return;

        dir.Normalize();

        // 장애물 체크: 플레이어가 밀리지 않도록 Raycast 두 개
        float checkDistance = moveSpeed * Time.fixedDeltaTime + 0.2f;
        Vector3 origin = rb.position + Vector3.up * 0.5f;

        // 중앙과 양쪽 레이 체크
        bool blocked = false;
        Vector3[] offsets = { Vector3.zero, transform.right * 0.4f, -transform.right * 0.4f };
        foreach (var offset in offsets)
        {
            if (Physics.Raycast(origin + offset, dir, out RaycastHit hit, checkDistance))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    blocked = true;
                    break;
                }
            }
        }

        if (!blocked)
        {
            Vector3 newPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            // 회전
            Quaternion targetRot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime));
        }
    }

    void RotateTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDir = hit.point - turret.position;
            lookDir.y = 0;
            turret.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    void Fire(InputAction.CallbackContext context)
    {
        if (Time.time < lastFireTime + fireCooldown) return;

        lastFireTime = Time.time;

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        OnFire?.Invoke();
    }
}