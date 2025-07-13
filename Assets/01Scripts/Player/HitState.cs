using UnityEngine;

public class HitState : BaseState<PlayerController>
{
    private float hitDuration = 0.3f;
    private float hitTimer = 0f;

    public HitState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        Owner.Animator.SetTrigger("IsHit");
        hitTimer = hitDuration;
        AudioManager.instance.PlaySFX("PlayerHit");
        // 이동, 공격 등의 입력 무시 상태 (애니메이션 루트 모션 or 멈춤)
    }

    public override void OnUpdateState()
    {
        hitTimer -= Time.deltaTime;
        if (hitTimer <= 0f)
        {
            Owner.ChangeToState(PLAYER_STATE.Move); // 스턴 끝
        }
    }

    public override void OnFixedUpdateState() { }

    public override void OnExitState()
    {
        // 아무것도 안 해도 됨
    }
}
