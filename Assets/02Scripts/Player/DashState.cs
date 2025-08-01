using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 dashDir;
    private float dashTimer;
    private bool isDashing;
    private float dashSpeed = 5;

    public DashState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.Animator.SetTrigger("IsDash");

        // 애니메이션 길이를 가져옵니다
        dashTimer = controller.Animator.GetCurrentAnimatorStateInfo(0).length;

        // 속도를 자동으로 계산하여 dashTimer 동안 DashDistance만큼 움직이도록 설정
        dashSpeed = controller.DashDistance / dashTimer;

        controller.Animator.SetFloat("DashSpeed", dashSpeed);

        Vector2 input = controller.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = controller.MainCamera.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = controller.MainCamera.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        if (moveDir.magnitude >= 0.1f)
        {
            dashDir = moveDir.normalized;
        }
        else
        {
            dashDir = controller.transform.forward;
        }

        isDashing = true;
    }


    public override void OnUpdateState()
    {
        if (!isDashing) return;

        dashTimer -= Time.deltaTime;

        Vector3 move = dashDir * controller.DashDistance;
        move.y = controller.VerticalVelocity;
        controller.Controller.Move(move * dashSpeed * Time.deltaTime);

        if (dashTimer <= 0f)
        {
            controller.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }

    public override void OnExitState()
    {
        isDashing = false;
    }

    public override void OnFixedUpdateState() { }
}
