using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("이동 설정")]
    [Tooltip("플레이어 이동 속도")]
    [SerializeField] float moveSpeed = 5f;

    [Tooltip("플레이어 회전 속도")]
    [SerializeField] float rotateSpeed = 180f;

    [Header("전투 설정")]
    [Tooltip("기본 공격력")]
    [SerializeField] float baseDamage = 2.5f;

    [Tooltip("투사체 프리팹")]
    [SerializeField] GameObject projectilePrefab;

    [Tooltip("발사 위치")]
    [SerializeField] Transform firePoint;

    [Tooltip("발사 힘")]
    [SerializeField] float fireForce = 20f;

    [Header("체력 설정")]
    [Tooltip("기본 체력")]
    [SerializeField] float maxHP = 12f;

    [Tooltip("사망 이펙트")]
    [SerializeField] GameObject deathEffect;

    float currentHP;
    Vector2 moveInput;
    Rigidbody rb;
    Camera mainCam;

    public float CurrentHP => currentHP;
    public float CurrentDamage => baseDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        currentHP = maxHP;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        RotateTurretToMouse();
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDir.magnitude < 0.1f) return;

        // 방향 회전
        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        rb.rotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRot,
            rotateSpeed * Time.fixedDeltaTime
        );

        // 앞으로 전진
        Vector3 forwardMove = transform.forward * moveDir.magnitude;

        rb.MovePosition(
            rb.position + forwardMove * moveSpeed * Time.fixedDeltaTime
        );
    }

    void RotateTurretToMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 lookDir = hit.point - firePoint.parent.position;
            lookDir.y = 0;

            firePoint.parent.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move Input: " + moveInput);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Shoot();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

        bullet.GetComponent<Projectile>().SetDamage(baseDamage);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}