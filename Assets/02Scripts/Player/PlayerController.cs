using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDamaged
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    [Header("Gravity Setting")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float terminalVelocity = -53f;

    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCoolTime = 1.0f;
    [SerializeField] private float dashEpAmount = 5.0f;

    private string dashAnimationName = "2Hand-Sword-DiveRoll-Forward1";
    public string DashAnimationName => dashAnimationName;
    [SerializeField] private float dashSpeed = 10f;
    public float DashSpeed => dashSpeed;


    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;

    private InputManager input;
    private WeaponManager weaponManager;

    private StateMachine<PLAYER_STATE, PlayerController> stateMachine;

    private float dashCoolDown = 0f;
    private float verticalVelocity;

    public static event Action<float> OnDashCooldownStart;

    public BaseWeapon CurrentWeapon => weaponManager.CurWeapon;
    public CharacterController Controller => controller;
    public Animator Animator { get => animator; set => animator = value; }
    public Transform MainCamera => mainCamera;
    public StateMachine<PLAYER_STATE, PlayerController> StateMachine => stateMachine;

    public Vector2 MoveInput => input.MoveInput;
    public WEAPON_TYPE CurrentWeaponType => weaponManager.CurrentWeaponType;

    public float VerticalVelocity => verticalVelocity;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float DashDistance => dashDistance;


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
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, terminalVelocity);
        }
    }

    public void TakeDamage(int amount)
    {
        var shield = GetComponent<Shield>();
        if (shield != null && shield.IsShieldActive())
        {
            int remaining = shield.AbsorbDamage(amount);
            if (remaining <= 0)
            {
                return;
            }

            amount = remaining;
        }

        PlayerStatManager.Instance.TakeDamage(amount);

        StateMachine.ChangeState(PLAYER_STATE.Hit);
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
        if (dashCoolDown > 0f) return;
        if (stateMachine.CurrentState is DashState) return;
        PlayerStatManager.Instance.ConsumeEP(dashEpAmount);

        dashCoolDown = dashCoolTime;

        OnDashCooldownStart?.Invoke(dashCoolTime);

        StartCoroutine(DashCoolTimeCO());

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

    private IEnumerator DashCoolTimeCO()
    {
        while(dashCoolDown > 0f)
        {
            dashCoolDown -= Time.deltaTime;
            yield return null;
        }
        dashCoolDown = 0f;
    }

    public void OnDashEnd()
    {
        // 현재 상태가 DashState일 때만 상태 전환
        if (StateMachine.CurrentState is DashState)
        {
            StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }

}
