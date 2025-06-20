using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float jumpHeight = 2.5f;
    public float gravity = -20f;

    private CharacterController controller;
    private Animator animator;
    private Camera cam;

    private float verticalVelocity;

    private float smoothMoveSpeed = 0f; 
    private float moveSpeedSmoothTime = 0.1f;
    private float moveSpeedVelocity;         

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // 카메라 기준 이동 방향
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 direction = (camForward * input.y + camRight * input.x).normalized;

        // 회전 처리
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // 점프 입력 처리
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                verticalVelocity = -1f;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // 이동 및 중력 적용
        Vector3 velocity = direction * moveSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        //애니메이션 - 부드러운 MoveSpeed 계산
        float targetSpeed = direction.magnitude;
        smoothMoveSpeed = Mathf.SmoothDamp(smoothMoveSpeed, targetSpeed, ref moveSpeedVelocity, moveSpeedSmoothTime);
        animator.SetFloat("MoveSpeed", smoothMoveSpeed);

        animator.SetBool("IsJumping", !controller.isGrounded);  // 정확하게 점프 상태 추적
    }
}
