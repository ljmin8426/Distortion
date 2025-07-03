using UnityEngine;

public class DashState : BaseState
{
    private Vector3 _dashDirection;
    private float _dashTimer;
    private bool _isDashing;

    public DashState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        _dashDirection = Controller.transform.forward;
        _dashTimer = Controller.DashDuration;  // 설정 가능하게 만들자
        _isDashing = true;

        AnimationEvents.OnDash?.Invoke();  // 애니메이션 재생용
    }

    public override void OnUpdateState()
    {
        if (!_isDashing) return;

        _dashTimer -= Time.deltaTime;

        Vector3 move = _dashDirection * Controller.DashSpeed;
        move.y = Controller.VerticalVelocity;

        Controller.Controller.Move(move * Time.deltaTime); //실제로 매 프레임 이동

        if (_dashTimer <= 0f)
        {
            _isDashing = false;
            Controller.ChangeToState(PLAYER_STATE.Move);
        }
    }

    public override void OnExitState()
    {
        _isDashing = false;
    }

    public override void OnFixedUpdateState() { }
}
