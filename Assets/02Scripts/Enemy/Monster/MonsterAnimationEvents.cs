using UnityEngine;

public class MonsterAnimationEvents : MonoBehaviour
{
    private IAttackTrigger attackTrigger;
    private MonsterBase monsterBase;

    private void Awake()
    {
        attackTrigger = GetComponentInChildren<IAttackTrigger>();
        monsterBase = GetComponentInParent<MonsterBase>();
    }

    public void OnAttackHit()
    {
        attackTrigger.Attack();
    }

    public void OnHitEnd()
    {
        attackTrigger.HitEnd();
    }

    public void OnAttackEnd()
    {
        monsterBase.ChangeState(Enemy_State.Chase);
    }

    public void OnDieEnd()
    {
        monsterBase.ReturnToPool();
    }
}
