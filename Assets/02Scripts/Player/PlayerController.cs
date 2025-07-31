using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    [Header("Gravity Setting")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float terminalVelocity = -53f;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 1.0f;
    [SerializeField] private float dashEpAmount = 5.0f;


    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;

    private InputManager input;
    private WeaponManager weaponManager;

    private StateMachine<PLAYER_STATE, PlayerController> stateMachine;

    private float dashCooldownTimer = 0f;
    private float _verticalVelocity;

    public static event Action<float> OnDashCooldownStart;

    public BaseWeapon CurrentWeapon => weaponManager.CurWeapon;
    public CharacterController Controller => controller;
    public Animator Animator { get => animator; set => animator = value; }
    public Transform MainCamera => mainCamera;
    public StateMachine<PLAYER_STATE, PlayerController> StateMachine => stateMachine;

    public Vector2 MoveInput => input.MoveInput;
    public WEAPON_TYPE CurrentWeaponType => weaponManager.CurrentWeaponType;

    public float VerticalVelocity => _verticalVelocity;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float DashSpeed => dashSpeed;


    #region Unity
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        input = FindAnyObjectByType<InputManager>();
        weaponManager = GetComponent<WeaponManager>();

        mainCamera = Camera.main.transform;
    }
    private void Start()
    {
        AddStates();
    }

    private void Update()
    {
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
    #endregion

    private void AddStates()
    {
        stateMachine = new StateMachine<PLAYER_STATE, PlayerController>(PLAYER_STATE.Move, new MoveState(this));
        stateMachine.AddState(PLAYER_STATE.Attack, new AttackState(this));
        stateMachine.AddState(PLAYER_STATE.Dash, new DashState(this));
        stateMachine.AddState(PLAYER_STATE.Hit, new HitState(this));
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
            _verticalVelocity = Mathf.Max(_verticalVelocity, terminalVelocity);
        }
    }

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

    private void OnDashInput()
    {
        if (dashCooldownTimer > 0f) return;
        if (stateMachine.CurrentState is DashState) return;
        PlayerStatManager.instance.ConsumeEP(dashEpAmount);

        dashCooldownTimer = dashCooldown;

        OnDashCooldownStart?.Invoke(dashCooldown); // UI에게 알림

        StateMachine.ChangeState(PLAYER_STATE.Dash);
    }

    private void OnAttackInput()
    {
        if (stateMachine.CurrentState is DashState || stateMachine.CurrentState is HitState) return;

        StateMachine.ChangeState(PLAYER_STATE.Attack);
    }

    private void OnSwapInput()
    {
        weaponManager.SwapWeapon();
    }
}
