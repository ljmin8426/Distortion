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
        dashSpeed = controller.DashSpeed;
        dashDistance = controller.DashDistance;
        dashEpCost = controller.DashEpAmount;
        dashCoolDown = controller.DashCoolDown;
        dashAnimName = controller.CurrentWeapon?.DashAnimationName;
    }

    public override bool CanEnter()
    {
        if (PlayerStatManager.Instance.CurrentEP < controller.DashEpAmount || isOnCooldown)
            return false;

        if (controller.StateMachine.CurrentState is AttackState)
            return false;

        return true;
    }

    public override void OnEnterState()
    {
        controller.Animator.SetTrigger("IsDash");

        OnDashCooldownStart?.Invoke(dashCoolDown);

        PlayerStatManager.Instance.ConsumeEP(dashEpCost);

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

        dashDuration = dashDistance / dashSpeed;

        float animLength = GetDashAnimationLength();
        float animSpeed = animLength > 0 ? animLength / dashDuration : 1f;
        controller.Animator.SetFloat("DashSpeed", animSpeed);

        controller.StartCoroutine(StartDashCooldown());

        verticalVelocity = controller.VerticalVelocity;
    }

    public override void OnUpdateState()
    {

        Vector3 move = dashDir * dashSpeed;
        move.y = verticalVelocity;
        controller.Controller.Move(move * Time.deltaTime);

        dashDuration -= Time.deltaTime;
        if (dashDuration <= 0f)
        {
            controller.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }

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
