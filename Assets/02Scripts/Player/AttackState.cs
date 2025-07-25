using UnityEngine;

public class AttackState : BaseState<PlayerController>
{
    private float exitDelay = 0.2f; // 공격 후 이동 상태로 넘어가기 전 딜레이
    private float exitTimer = 0f;
    private bool fired = false;

    public AttackState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        Owner.LookAtCursor();

        switch (Owner.CurrentWeaponType)
        {
            case WEAPON_TYPE.Melee:
                Owner.Animator.SetTrigger("IsAttack");
                Owner.Animator.SetFloat("AttackSpeed", Owner.CurrentWeapon.WeaponData.attackSpeed + PlayerStatManager.instance.AttackSpeed);
                break;

            case WEAPON_TYPE.Range:
                Owner.Animator.SetTrigger("IsFire");
                break;
        }

        Owner.TryAttack();  // 무기 발사
        fired = true;
        exitTimer = 0f;
    }

    public override void OnUpdateState()
    {
        if (Owner.CurrentWeaponType == WEAPON_TYPE.Range && fired)
        {
            exitTimer += Time.deltaTime;
            if (exitTimer >= exitDelay)
            {
                Owner.ChangeToState(PLAYER_STATE.Move);
            }
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}
