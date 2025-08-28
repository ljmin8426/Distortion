using System;
using System.Collections;
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
    [SerializeField] private float groundedGravity = -2f;


    private float verticalVelocity;

    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;
    private PlayerInputManager input;
    private WeaponManager weaponManager;
    private StateMachine<PLAYER_STATE, PlayerController> stateMachine;

    private Mouse mouse;

    public BaseWeapon CurrentWeapon => weaponManager.CurWeapon;
    public CharacterController Controller => controller;
    public Animator Animator => animator;
    public Transform MainCamera => mainCamera;
    public StateMachine<PLAYER_STATE, PlayerController> StateMachine => stateMachine;
    public Vector2 MoveInput => input.MoveInput;
    public WEAPON_TYPE CurrentWeaponType => weaponManager.CurrentWeaponType;
    public float VerticalVelocity => verticalVelocity;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;


    #region Unity Lifecycle
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        input = FindAnyObjectByType<PlayerInputManager>();
        weaponManager = GetComponent<WeaponManager>();

        if (Camera.main != null)
            mainCamera = Camera.main.transform;

        mouse = Mouse.current;
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
        PlayerInputManager.OnAttack += OnAttackInput;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnAttack -= OnAttackInput;
    }
    #endregion

    private void AddStates()
    {
        stateMachine = new StateMachine<PLAYER_STATE, PlayerController>(PLAYER_STATE.Move, new MoveState(this));
        stateMachine.AddState(PLAYER_STATE.Attack, new AttackState(this));
        stateMachine.AddState(PLAYER_STATE.Dash, new DashState(this));
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = groundedGravity;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity)
                verticalVelocity = terminalVelocity;
        }
    }

    public void LookAtCursor()
    {
        Vector3 mousePosition = mouse.position.ReadValue();
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
        StateMachine.ChangeState(PLAYER_STATE.Dash);
    }

    private void OnAttackInput()
    {
        StateMachine.ChangeState(PLAYER_STATE.Attack);
    }
}
