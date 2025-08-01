using UnityEngine;
public class AttackState : BaseState<PlayerController>
{
    private float exitDelay = 0.2f; // 공격 후 이동 상태로 넘어가기 전 딜레이
    private float exitTimer = 0f;
    private bool fired = false;

    public AttackState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.LookAtCursor();

        switch (controller.CurrentWeaponType)
        {
            case WEAPON_TYPE.Melee:
                controller.Animator.SetTrigger("IsAttack");
                controller.Animator.SetFloat("AttackSpeed", controller.CurrentWeapon.WeaponData.attackSpeed + PlayerStatManager.Instance.AttackSpeed);
                break;

            case WEAPON_TYPE.Range:
                controller.Animator.SetTrigger("IsFire");
                break;
        }

        controller.CurrentWeapon.Attack();  // 무기 발사
        fired = true;
        exitTimer = 0f;
    }

    public override void OnUpdateState()
    {
        if (controller.CurrentWeaponType == WEAPON_TYPE.Range && fired)
        {
            exitTimer += Time.deltaTime;
            if (exitTimer >= exitDelay)
            {
                controller.StateMachine.ChangeState(PLAYER_STATE.Move);
            }
        }
    }

    public override void OnFixedUpdateState() { }
    public override void OnExitState() { }
}
