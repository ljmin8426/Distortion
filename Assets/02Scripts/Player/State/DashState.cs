using UnityEngine;
using System.Collections;
using System;

public class DashState : BaseState<PlayerController>
{
    private Vector3 dashDir;
    private float dashSpeed;
    private float dashDuration;
    private float dashCoolDown;
    private bool isOnCooldown;
    private float verticalVelocity; // 중력 처리용 캐싱

    // 한 번만 가져오면 되는 값들 캐싱
    private float dashDistance;
    private string dashAnimName;
    private float dashEpCost;

    public static event Action<float> OnDashCooldownStart;

    public DashState(PlayerController controller) : base(controller)
    {
        // 캐싱
        dashSpeed = controller.DashSpeed;
        dashDistance = controller.DashDistance;
        dashEpCost = controller.DashEpAmount;
        dashCoolDown = controller.DashCoolDown;
        // 무기에서 애니메이션 이름과 길이 받아오기
        dashAnimName = controller.CurrentWeapon?.DashAnimationName ?? "DefaultDashAnim";
    }

    public override bool CanEnter()
    {
        // EP 부족, 쿨타임 중은 불가
        if (PlayerStatManager.Instance.CurrentEP < controller.DashEpAmount || isOnCooldown)
            return false;

        // 공격 상태일 때 대시 불가
        if (controller.StateMachine.CurrentState is AttackState)
            return false;

        return true;
    }

    public override void OnEnterState()
    {
        controller.Animator.SetTrigger("IsDash");

        OnDashCooldownStart?.Invoke(dashCoolDown);

        // EP 차감
        PlayerStatManager.Instance.ConsumeEP(dashEpCost);

        // 방향 계산
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

        // 대시 지속 시간
        dashDuration = dashDistance / dashSpeed;

        // 애니메이션 속도 동기화
        float animLength = GetDashAnimationLength();
        float animSpeed = animLength > 0 ? animLength / dashDuration : 1f;
        controller.Animator.SetFloat("DashSpeed", animSpeed);

        // 쿨타임 시작
        controller.StartCoroutine(StartDashCooldown());

        // 중력 초기화
        verticalVelocity = controller.VerticalVelocity;
    }

    public override void OnUpdateState()
    {
        ApplyGravity();

        Vector3 move = dashDir * dashSpeed;
        move.y = verticalVelocity;
        controller.Controller.Move(move * Time.deltaTime);

        // 대시 종료 타이밍
        dashDuration -= Time.deltaTime;
        if (dashDuration <= 0f)
        {
            controller.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }

    private void ApplyGravity()
    {
        if (controller.Controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // 바닥 붙잡기
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    private float GetDashAnimationLength()
    {
        var clips = controller.Animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == dashAnimName)
                return clip.length;
        }
        return 0.5f;
    }

    private IEnumerator StartDashCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(dashCoolDown);
        isOnCooldown = false;
    }
}
