using System;
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
    [SerializeField] private float terminalVelocity = -53f;

    [Header("Weapon Handler")]
    [SerializeField] private Transform weaponHolder;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 1.0f;
    [SerializeField] private float dashEpAmount = 5.0f;


    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;

    private InputManager input;
    private WeaponManager weaponManager;

    private StateMachine stateMachine;

    private float dashCooldownTimer = 0f;
    private float _verticalVelocity;
    private bool _isGrounded;

    public static event Action<float> OnDashCooldownStart;

    public BaseWeapon CurrentWeapon => weaponManager.CurWeapon;
    public CharacterController Controller => controller;
    public Animator Animator { get => animator; set => animator = value; }
    public Transform MainCamera => mainCamera;
    public StateMachine StateMachine => stateMachine;

    public Vector2 MoveInput => input.MoveInput;
    public WEAPON_TYPE CurrentWeaponType => weaponManager.CurrentWeaponType;

    public float VerticalVelocity => _verticalVelocity;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float DashSpeed => dashSpeed;

    public void SetVerticalVelocity(float v) => _verticalVelocity = v;

    private void Awake()
    {

        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main.transform;

        input = GetComponent<InputManager>();
        weaponManager = GetComponent<WeaponManager>();

        stateMachine = new StateMachine(PLAYER_STATE.Move, new MoveState(this));
        stateMachine.AddState(PLAYER_STATE.Attack, new AttackState(this));
        stateMachine.AddState(PLAYER_STATE.Dash, new DashState(this));
    }

    private void Start()
    {
        weaponManager.Initialized(weaponHolder);
    }

    private void Update()
    {
        _isGrounded = controller.isGrounded;
        ApplyGravity();

        // 쿨타임 감소
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        stateMachine.UpdateState();
    }


    private void FixedUpdate()
    {
        stateMachine.FixedUpdateState();
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
        if (dashCooldownTimer > 0f) return;
        if (stateMachine.CurrentState is DashState) return;
        PlayerStatManager.instance.ConsumeEP(dashEpAmount);

        dashCooldownTimer = dashCooldown;

        OnDashCooldownStart?.Invoke(dashCooldown); // UI에게 알림

        ChangeToState(PLAYER_STATE.Dash);
    }


    private void OnAttackInput()
    {
        // 대시 중에는 공격 못 함
        if (stateMachine.CurrentState is DashState) return;

        ChangeToState(PLAYER_STATE.Attack);
    }
    private void OnSwapInput() => weaponManager.SwapWeapon();

    public void TryAttack() => weaponManager.TryAttack();

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

    public void ChangeToState(PLAYER_STATE next) => stateMachine.ChangeState(next);

    private void ApplyGravity()
    {
        if (_isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
            _verticalVelocity = Mathf.Max(_verticalVelocity, terminalVelocity);
        }
    }

    public T GetState<T>(PLAYER_STATE stateName) where T : BaseState
    {
        return stateMachine.GetState(stateName) as T;
    }
}
