using UnityEngine;

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
    private PlayerAnimatorController _skillAnimator;
    private Animator _animator;
    private Transform _mainCamera;
    private IPlayerSkill _skill;

    private float _speed;
    private float _animationBlend;
    private const float SpeedChangeRate = 10.0f;
    private const float speedThreshold = 0.01f;
    private int _moveSpeedHash = Animator.StringToHash("MoveSpeed");

    private float _verticalVelocity;
    private float _terminalVelocity = -53f;

    private bool _isGrounded;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputController>();
        _skillAnimator = GetComponent<PlayerAnimatorController>();
        _animator = GetComponentInChildren<Animator>();
        _mainCamera = Camera.main.transform;
        _skill = GetComponent<IPlayerSkill>();
    }

    private void Update()
    {
        GroundCheck();
        ApplyGravity();
        _skill?.TryUseSkill(_input, _skillAnimator, _mainCamera, transform, _controller);
        Move();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, groundedCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
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

    private void Move()
    {
        Vector2 moveInput = _input.MoveInput;
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        float targetSpeed = moveInput == Vector2.zero ? 0f : moveSpeed;

        // 속도 보간
        if (Mathf.Abs(_speed - targetSpeed) > speedThreshold)
        {
            _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * SpeedChangeRate);
        }
        else
        {
            _speed = targetSpeed;
        }

        Vector3 move = Vector3.zero;

        if (inputDir.magnitude >= 0.1f)
        {
            Vector3 camForward = _mainCamera.forward; camForward.y = 0f;
            Vector3 camRight = _mainCamera.right; camRight.y = 0f;
            Vector3 moveDir = camForward.normalized * inputDir.z + camRight.normalized * inputDir.x;

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            move = moveDir.normalized * _speed;
        }

        // 이동 + 중력 적용
        move.y = _verticalVelocity;
        _controller.Move(move * Time.deltaTime);

        // 애니메이션 블렌딩
        float targetBlend = moveInput.magnitude;
        _animationBlend = Mathf.Lerp(_animationBlend, targetBlend, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        if (Mathf.Abs(_animator.GetFloat(_moveSpeedHash) - _animationBlend) > speedThreshold)
        {
            _animator.SetFloat(_moveSpeedHash, _animationBlend);
        }
    }
}
