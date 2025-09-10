using UnityEngine;

public class DashState : BaseState<PlayerCtrl>
{
    private float dashSpeed = 20f;
    private float dashDuration = 0.2f;
    private float dashTimer;
    private Vector3 dashDirection;
    public bool isDash { get; private set; } = false;

    public DashState(PlayerCtrl controller) : base(controller) { }

    public override bool CanEnter()
    {
        if (!PlayerStatManager.Instance.UseDash())
            return false; // 대시 없음

        // 공격 중이면 대시 불가
        if (owner.StateMachine.CurrentState is AttackState)
            return false;

        // 플레이어가 이동 중일 때만 대시 가능
        if (owner.MoveInput.magnitude <= 0.1f)
            return false;

        return true;
    }

    public override void OnEnterState()
    {
        isDash = true;
        dashTimer = dashDuration;

        AudioManager.Instance.PlaySoundFXClip(owner.DashSound, owner.transform, 1f);

        Vector2 input = owner.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = owner.MainCamera.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = owner.MainCamera.right;
        camRight.y = 0;
        camRight.Normalize();

        dashDirection = (inputDir.magnitude >= 0.1f)
            ? camForward * inputDir.z + camRight * inputDir.x
            : owner.transform.forward;

        dashDirection.Normalize();
    }

    public override void OnUpdateState()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            owner.StateMachine.ChangeState(PLAYER_STATE.Move);
        }

        if (dashDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dashDirection);
            owner.transform.rotation = Quaternion.Slerp(
                owner.transform.rotation,
                targetRot,
                owner.RotationSpeed * Time.deltaTime
            );
        }
    }

    public override void OnFixedUpdateState()
    {
        if (!isDash) return;

        Vector3 move = dashDirection * dashSpeed + Vector3.up * owner.VerticalVelocity;
        owner.Controller.Move(move * Time.fixedDeltaTime);
    }

    public override void OnExitState()
    {
        isDash = false;
    }
}
