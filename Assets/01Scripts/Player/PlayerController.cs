using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8.0f;
    public float rotationSpeed = 10.0f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundedOffset = -0.1f;
    public float groundedCheckRadius = 0.3f;
    public LayerMask groundLayers;

    private CharacterController _controller;
    private PlayerInputController _input;
    private Animator _animator;
    private Transform _mainCamera;
    private WeaponManager _weaponManager;

    private float _verticalVelocity;
    private float _terminalVelocity = -53f;
    private bool _isGrounded;

    private IPlayerState _currentState;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputController>();
        _animator = GetComponentInChildren<Animator>();
        _mainCamera = Camera.main.transform;
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void OnEnable()
    {
        PlayerInputController.OnAttack += OnAttackInput;
        PlayerInputController.OnUtil += OnDashInput;
    }

    private void OnDisable()
    {
        PlayerInputController.OnAttack -= OnAttackInput;
        PlayerInputController.OnUtil -= OnDashInput;
    }

    public void TryAttack()
    {
        _weaponManager?.TryAttack();  // 무기 기반 공격 실행
    }

    private void Start()
    {
        ChangeState(new PlayerMoveState());
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;
        ApplyGravity();
        _currentState?.Update();
    }

    private void OnAttackInput()
    {
        if (_currentState is PlayerMoveState)
        {
            ChangeState(new PlayerAttackState());
        }
    }

    private void OnDashInput()
    {
        if (_currentState is PlayerMoveState)
        {
            ChangeState(new PlayerDashState());
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter(this);
    }

    private void ApplyGravity()
    {
        if (_isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity = _terminalVelocity;
        }
    }
    public void TryLook(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = lookRot;
        }
    }

    public Vector3 GetMouseWorldDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 target = hit.point;
            Vector3 myPos = transform.position;
            target.y = myPos.y; // 수평만 고려
            return (target - myPos).normalized;
        }

        return transform.forward; // 실패 시 전방 유지
    }

    // 상태에서 접근할 수 있도록 public getter 제공
    public CharacterController Controller => _controller;
    public Animator Animator => _animator;
    public Transform MainCamera => _mainCamera;
    public Vector2 MoveInput => _input.MoveInput;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float VerticalVelocity => _verticalVelocity;
    public void SetVerticalVelocity(float v) => _verticalVelocity = v;
    public bool IsGrounded => _isGrounded;
}
