using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    [Header("Gravity Setting")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float _terminalVelocity = -53f;

    [Header("Weapon Handler")]
    [SerializeField] private Transform weaponHolder;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.0f;

    private float _dashCooldownTimer = 0f;

    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;
    public float DashCooldown => dashCooldown;


    private CharacterController _controller;
    private Animator _animator;
    private InputManager _input;
    private WeaponManager _weaponManager;
    private Transform _mainCamera;

    private StateMachine _stateMachine;

    private float _verticalVelocity;
    private bool _isGrounded;

    public BaseWeapon CurrentWeapon => _weaponManager.CurWeapon;
    public CharacterController Controller => _controller;
    public Animator Animator { get => _animator; set => _animator = value; }
    public Transform MainCamera => _mainCamera;
    public Vector2 MoveInput => _input.MoveInput;
    public float VerticalVelocity => _verticalVelocity;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public WEAPON_TYPE CurrentWeaponType => _weaponManager.CurrentWeaponType;

    public void SetVerticalVelocity(float v) => _verticalVelocity = v;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<InputManager>();
        _animator = GetComponentInChildren<Animator>();
        _weaponManager = GetComponent<WeaponManager>();
        _mainCamera = Camera.main.transform;

        _stateMachine = new StateMachine(PLAYER_STATE.Move, new MoveState(this));
        _stateMachine.AddState(PLAYER_STATE.Attack, new AttackState(this));
        _stateMachine.AddState(PLAYER_STATE.Dash, new DashState(this));
    }

    private void Start()
    {
        _weaponManager.Initialized(weaponHolder, _animator);
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;
        ApplyGravity();

        // 쿨타임 감소
        if (_dashCooldownTimer > 0f)
            _dashCooldownTimer -= Time.deltaTime;

        _stateMachine.UpdateState();
    }


    private void FixedUpdate()
    {
        _stateMachine.FixedUpdateState();
    }

    private void OnEnable()
    {
        InputManager.OnAttack += OnAttackInput;
        InputManager.OnUtil += OnDashInput;
        InputManager.OnSwap += OnSwapInput;
    }

    private void OnDisable()
    {
        InputManager.OnAttack -= OnAttackInput;
        InputManager.OnUtil -= OnDashInput;
        InputManager.OnSwap -= OnSwapInput;
    }

    private void OnDashInput()
    {
        // 쿨타임 중이면 무시
        if (_dashCooldownTimer > 0f) return;

        // 대시 중이면 무시
        if (_stateMachine.CurrentState is DashState) return;

        // 대시 시작 + 쿨타임 초기화
        _dashCooldownTimer = dashCooldown;
        ChangeToState(PLAYER_STATE.Dash);
    }

    private void OnAttackInput()
    {
        // 대시 중에는 공격 못 함
        if (_stateMachine.CurrentState is DashState) return;

        ChangeToState(PLAYER_STATE.Attack);
    }
    private void OnSwapInput() => _weaponManager.SwapWeapon();

    public void TryAttack() => _weaponManager.TryAttack();

    public void LookAtCursor()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;
            Vector3 dir = (target - transform.position).normalized;
            if (dir.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void ChangeToState(PLAYER_STATE next) => _stateMachine.ChangeState(next);

    private void ApplyGravity()
    {
        if (_isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
            _verticalVelocity = Mathf.Max(_verticalVelocity, _terminalVelocity);
        }
    }

    public T GetState<T>(PLAYER_STATE stateName) where T : BaseState
    {
        return _stateMachine.GetState(stateName) as T;
    }

}
