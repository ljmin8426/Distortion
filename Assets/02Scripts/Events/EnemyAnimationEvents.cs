using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyBase enemyBase;

    private void Awake()
    {
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    // 공격 타격 시점에 호출되는 애니메이션 이벤트
    public void OnAttackHit()
    {
        enemyBase.AttackCollider?.EnableCollider();
    }

    // 타격 끝 지점 호출 이벤트
    public void OnHitEnd()
    {
        enemyBase.AttackCollider?.DisableCollider();
    }

    // 공격 애니메이션 끝날 때 호출
    public void OnAttackEnd()
    {
        float dist = Vector3.Distance(enemyBase.transform.position, enemyBase.player.position);
        if (dist <= enemyBase.EnemyData.attackRange)
            enemyBase.ChangeState(ENEMY_STATE.Attack);
        else
            enemyBase.ChangeState(ENEMY_STATE.Chase);
    }

    // 사망모션 후 삭제
    public void OnDieEnd()
    {
        enemyBase.DestroyEnemy();
    }
}
