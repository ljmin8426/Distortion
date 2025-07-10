using UnityEngine;

public class DashState : BaseState
{
    private Vector3 _dashDirection;
    private float _dashTimer;
    private bool _isDashing;

    public DashState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        Controller.Animator.SetTrigger("IsDash");
        Controller.Animator.SetFloat("DashSpeed", 3);
        float curAnimTime = Controller.Animator.GetCurrentAnimatorStateInfo(0).length;
        _dashDirection = Controller.transform.forward;
        _dashTimer = curAnimTime;
        _isDashing = true;
    }

    public override void OnUpdateState()
    {
        if (!_isDashing) return;

        _dashTimer -= Time.deltaTime;

        Vector3 move = _dashDirection * Controller.DashSpeed;
        move.y = Controller.VerticalVelocity;
        Controller.Controller.Move(move * Time.deltaTime);

        if (_dashTimer <= 0f)
        {
            _isDashing = false;
            Controller.ChangeToState(PLAYER_STATE.Move);
        }
    }

    public override void OnExitState() => _isDashing = false;

    public override void OnFixedUpdateState() { }
}
