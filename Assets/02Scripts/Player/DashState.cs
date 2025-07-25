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

        dashDir = controller.transform.forward;
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
