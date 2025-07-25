using UnityEngine;

public class HitState : BaseState<PlayerController>
{
    private float hitDuration = 0.1f;
    private float hitTimer = 0f;

    public HitState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.Animator.SetTrigger("IsHit");
        hitTimer = hitDuration;
    }

    public override void OnUpdateState()
    {
        hitTimer -= Time.deltaTime;
        if (hitTimer <= 0f)
        {
            controller.StateMachine.ChangeState(PLAYER_STATE.Move);
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}
