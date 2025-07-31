using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 dashDir;
    private float dashTimer;
    private bool isDashing;

    public DashState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.Animator.SetTrigger("IsDash");
        controller.Animator.SetFloat("DashSpeed", 3);

        float curAnimTime = controller.Animator.GetCurrentAnimatorStateInfo(0).length;

        // 카메라 기준 입력 방향을 dashDir로 사용
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
            // 입력이 없으면 정면
            dashDir = controller.transform.forward;
        }

        dashTimer = curAnimTime;
        isDashing = true;
    }


    public override void OnUpdateState()
    {
        if (!isDashing) return;

        dashTimer -= Time.deltaTime;

        Vector3 move = dashDir * controller.DashSpeed;
        move.y = controller.VerticalVelocity;
        controller.Controller.Move(move * Time.deltaTime);

        if (dashTimer <= 0f)
        {
            isDashing = false;
            controller.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }

    public override void OnExitState()
    {
        isDashing = false;
    }

    public override void OnFixedUpdateState() { }
}
