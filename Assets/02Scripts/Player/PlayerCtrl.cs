using UnityEngine;
using UnityEngine.InputSystem;

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

    private CharacterController controller;
    private Animator animator;
    private Transform mainCamera;
    private PlayerInputManager input;
    private WeaponManager weaponManager;
    private StateMachine<Player_State, PlayerCtrl> stateMachine;

    private Mouse mouse;

    #region Anim Hash
    private int isAttack = Animator.StringToHash("isAttack");

    private int speed = Animator.StringToHash("moveSpeed");
    public int IsMove => IsMove;
    public int Speed => speed;
    #endregion

    public AudioClip DashSound => dashSound;
    public CharacterController Controller => controller;
    public Animator Animator => animator;
    public Transform MainCamera => mainCamera;

    public StateMachine<Player_State, PlayerCtrl> StateMachine => stateMachine;
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
        animator = GetComponent<Animator>();
            
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
        stateMachine = new StateMachine<Player_State, PlayerCtrl>(Player_State.Move, new MoveState(this));
        stateMachine.AddState(Player_State.Attack, new AttackState(this));
        stateMachine.AddState(Player_State.Dash, new DashState(this));
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
        if (MoveInput.magnitude <= 0.1f) return;

        StateMachine.ChangeState(Player_State.Dash);
    }
                
    private void OnAttackInput()
    {
        Animator.SetTrigger(isAttack);
        stateMachine.ChangeState(Player_State.Attack);
    }
}
