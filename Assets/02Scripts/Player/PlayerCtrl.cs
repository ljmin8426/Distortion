using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(CharacterController))]
public class PlayerCtrl : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotationSpeed = 10.0f;

    [Header("Gravity Setting")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float terminalVelocity = -53f;
    [SerializeField] private float groundedGravity = -2f;

    [Header("Sound")]
    [SerializeField] private AudioClip dashSound;

    private float verticalVelocity;
    private bool isStop;

    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;
    private PlayerInputManager input;
    private WeaponManager weaponManager;
    private StateMachine<PLAYER_STATE, PlayerCtrl> stateMachine;

    private Mouse mouse;

    public AudioClip DashSound => dashSound;
    public CharacterController Controller => controller;
    public Animator Animator => animator;
    public Transform MainCamera => mainCamera;

    public StateMachine<PLAYER_STATE, PlayerCtrl> StateMachine => stateMachine;
    public WeaponManager WeaponManager => weaponManager;

    public Vector2 MoveInput => input.MoveInput;

    public float VerticalVelocity => verticalVelocity;
    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;

    #region Unity Lifecycle
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        weaponManager = GetComponent<WeaponManager>();
        input = GetComponent<PlayerInputManager>();
        animator = GetComponentInChildren<Animator>();

        if (Camera.main != null)
            mainCamera = Camera.main.transform;

        mouse = Mouse.current;
    }

    private void Start()
    {
        InitPlayerState();
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
        PlayerInputManager.OnUtil += OnDashInput;
    }

    private void OnDisable()
    {
        if(input != null)
        {
            PlayerInputManager.OnAttack -= OnAttackInput;
            PlayerInputManager.OnUtil -= OnDashInput;
        }

    }
    #endregion

    private void InitPlayerState()
    {
        stateMachine = new StateMachine<PLAYER_STATE, PlayerCtrl>(PLAYER_STATE.Move, new MoveState(this));
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
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(mousePosition);


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
        if (isStop) return;



        // 이동속도가 없으면 대시 불가
        if (MoveInput.magnitude <= 0.1f) return;

        StateMachine.ChangeState(PLAYER_STATE.Dash);
    }

    private void OnAttackInput()
    {
        if (isStop) return;

        Animator.SetTrigger("isAttack");

        stateMachine.ChangeState(PLAYER_STATE.Attack);
    }

    public void SetMove(bool canMove)
    {
        isStop = canMove;
        if (!isStop) 
            animator.SetFloat("moveSpeed", 0f);
    }
}
