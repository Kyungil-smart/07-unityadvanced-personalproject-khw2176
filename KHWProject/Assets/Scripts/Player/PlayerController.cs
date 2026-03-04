using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("이동 속도")]
    [SerializeField] private float moveSpeed = 5f;

    [Tooltip("회전 속도")]
    [SerializeField] private float rotateSpeed = 180f;

    [Header("포신 설정")]
    [SerializeField] private Transform turret;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private Rigidbody rb;
    private TankInput input;
    private Vector2 moveInput;

    public event Action OnFire;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        Debug.Log($"moveInput = {moveInput}, phase = {ctx.phase}");
    }

    void Move()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rot, rotateSpeed * Time.fixedDeltaTime));
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
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        OnFire?.Invoke();
    }
}