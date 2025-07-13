using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 dashDir;
    private float dashTimer;
    private bool isDashing;

    public DashState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        Owner.Animator.SetTrigger("IsDash");
        AudioManager.instance.PlaySFX("Dash");
        Owner.Animator.SetFloat("DashSpeed", 3);
        float curAnimTime = Owner.Animator.GetCurrentAnimatorStateInfo(0).length;
        dashDir = Owner.transform.forward;
        dashTimer = curAnimTime;
        isDashing = true;
    }

    public override void OnUpdateState()
    {
        if (!isDashing) return;

        dashTimer -= Time.deltaTime;

        Vector3 move = dashDir * Owner.DashSpeed;
        move.y = Owner.VerticalVelocity;
        Owner.Controller.Move(move * Time.deltaTime);

        if (dashTimer <= 0f)
        {
            isDashing = false;
            Owner.ChangeToState(PLAYER_STATE.Move);
        }
    }

    public override void OnExitState() => isDashing = false;

    public override void OnFixedUpdateState() { }
}
