using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private float dashSpeed = 20f;
    private float dashDuration = 0.2f;
    private float dashTimer;
    private Vector3 dashDirection;
    public bool isDash { get; private set; } = false;

    public DashState(PlayerController controller) : base(controller) { }

    public override bool CanEnter()
    {
        return owner.Controller.isGrounded && !isDash;
    }

    public override void OnEnterState()
    {
        isDash = true;
        dashTimer = dashDuration;

        AudioManager.Instance.PlaySoundFXClip(owner.DashSound, owner.transform, 1f);

        // 카메라 기준 입력 계산
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

        // 캐릭터 회전
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

        // 데쉬 이동 + 중력 적용
        Vector3 move = dashDirection * dashSpeed + Vector3.up * owner.VerticalVelocity;
        owner.Controller.Move(move * Time.fixedDeltaTime);
    }

    public override void OnExitState()
    {
        isDash = false;
    }
}
