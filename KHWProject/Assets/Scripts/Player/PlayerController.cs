using UnityEngine;
using UnityEngine.InputSystem;
using System;

//
// Rigidbody 컴포넌트가 반드시 있어야 한다는 의미
// 없으면 자동으로 추가됨
//
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    // 탱크 이동 속도
    [SerializeField] private float moveSpeed = 5f;

    // 탱크 몸체 회전 속도
    [SerializeField] private float rotateSpeed = 180f;


    [Header("포신 설정")]
    // 포신(터렛) Transform
    // 마우스를 따라 회전하는 부분
    [SerializeField] private Transform turret;

    // 발사할 탄환 프리팹
    [SerializeField] private GameObject projectilePrefab;

    // 탄환이 생성될 위치
    [SerializeField] private Transform firePoint;


    // 발사 쿨타임 (연속 발사 방지)
    [SerializeField] private float fireCooldown = 0.4f;

    // 마지막 발사 시간 저장
    private float lastFireTime;


    // Rigidbody 참조 (물리 이동)
    private Rigidbody rb;

    // Input System에서 생성된 입력 클래스
    private TankInput input;

    // WASD 입력 값 저장
    private Vector2 moveInput;


    // 발사 이벤트 (다른 스크립트에서 사용 가능)
    public event Action OnFire;


    private void Awake()
    {
        // Rigidbody 가져오기
        rb = GetComponent<Rigidbody>();

        // 물리로 인한 회전 방지
        rb.freezeRotation = true;

        // Input System 초기화
        input = new TankInput();
    }


    private void OnEnable()
    {
        // Input Action 활성화
        input.Player.Enable();

        // 이동 입력 발생
        input.Player.Move.performed += OnMove;

        // 이동 입력 종료
        input.Player.Move.canceled += OnMove;

        // 발사 입력
        input.Player.Fire.performed += Fire;
    }


    private void OnDisable()
    {
        // 이벤트 제거 (메모리 누수 방지)
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Fire.performed -= Fire;

        input.Player.Disable();
    }


    private void Update()
    {
        // 마우스를 따라 포신 회전
        RotateTurret();
    }


    private void FixedUpdate()
    {
        // 물리 이동 처리
        Move();
    }


    // 이동 입력 받을 때 호출
    void OnMove(InputAction.CallbackContext ctx)
    {
        // WASD 입력 값을 Vector2로 저장
        moveInput = ctx.ReadValue<Vector2>();
    }


    void Move()
    {
        // 2D 입력을 3D 방향으로 변환
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);

        // 입력이 거의 없으면 이동하지 않음
        if (dir.sqrMagnitude < 0.01f) return;

        // 방향 정규화 (속도 일정하게 유지)
        dir.Normalize();



        // 장애물 충돌 체크 (Raycast)

        // 이동 거리 계산
        float checkDistance = moveSpeed * Time.fixedDeltaTime + 0.2f;

        // Ray 시작 위치
        Vector3 origin = rb.position + Vector3.up * 0.5f;


        // 중앙 + 좌우 Raycast
        bool blocked = false;

        Vector3[] offsets =
        {
            Vector3.zero,                 // 중앙
            transform.right * 0.4f,       // 오른쪽
            -transform.right * 0.4f       // 왼쪽
        };


        // Raycast 검사
        foreach (var offset in offsets)
        {
            if (Physics.Raycast(origin + offset, dir, out RaycastHit hit, checkDistance))
            {
                // Obstacle 레이어면 이동 차단
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    blocked = true;
                    break;
                }
            }
        }


        // 실제 이동
        if (!blocked)
        {
            // Rigidbody 기반 이동
            Vector3 newPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            // 이동 방향으로 몸체 회전
            Quaternion targetRot = Quaternion.LookRotation(dir);

            rb.MoveRotation(
                Quaternion.RotateTowards(
                    rb.rotation,
                    targetRot,
                    rotateSpeed * Time.fixedDeltaTime
                )
            );
        }
    }


    void RotateTurret()
    {
        // 마우스 위치에서 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        // Ray가 바닥에 맞으면
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 포신에서 마우스 위치까지 방향
            Vector3 lookDir = hit.point - turret.position;

            // 포신이 위아래로 기울지 않게 Y 제거
            lookDir.y = 0;

            // 포신 회전
            turret.rotation = Quaternion.LookRotation(lookDir);
        }
    }


    void Fire(InputAction.CallbackContext context)
    {
        // 쿨타임 체크
        if (Time.time < lastFireTime + fireCooldown)
            return;

        // 발사 시간 기록
        lastFireTime = Time.time;

        // 탄환 생성
        Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        // 발사 이벤트 호출
        OnFire?.Invoke();
    }
}