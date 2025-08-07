using UnityEngine;

public class DashState : BaseState<PlayerController>
{
    private Vector3 dashDir;
    private float dashSpeed;
    private float dashDuration;

    public DashState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.Animator.SetTrigger("IsDash");

        Vector2 input = controller.MoveInput;
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 camForward = controller.MainCamera.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = controller.MainCamera.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        dashDir = moveDir.magnitude >= 0.1f ? moveDir.normalized : controller.transform.forward;

        // 실제 대시 속도 설정 (코드에서 설정한 고정값)
        dashSpeed = controller.DashSpeed;

        // 이동 시간 계산
        dashDuration = controller.DashDistance / dashSpeed;

        // 애니메이션 길이 가져오기
        float animLength = GetDashAnimationLength();

        // 애니메이션 재생 속도 설정 → 애니메이션이 이동 시간과 정확히 맞도록
        float animSpeed = animLength > 0 ? animLength / dashDuration : 1f;
        controller.Animator.SetFloat("DashSpeed", animSpeed);
    }

    public override void OnUpdateState()
    {
        Vector3 move = dashDir * dashSpeed;
        move.y = controller.VerticalVelocity;
        controller.Controller.Move(move * Time.deltaTime);
    }

    public override void OnExitState() { }

    public override void OnFixedUpdateState() { }

    private float GetDashAnimationLength()
    {
        var clips = controller.Animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == controller.DashAnimationName)
                return clip.length;
        }

        Debug.LogWarning("Dash 애니메이션 클립을 찾을 수 없습니다. 기본값 사용");
        return 0.5f; // 기본값
    }
}
