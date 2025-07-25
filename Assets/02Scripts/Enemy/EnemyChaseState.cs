using UnityEngine;

public class EnemyChaseState : BaseState<EnemyBase>
{
    public EnemyChaseState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        Owner.agent.isStopped = false;
    }

    public override void OnUpdateState()
    {
        float distance = Vector3.Distance(Owner.transform.position, Owner.player.position);

        if (distance <= Owner.EnemyData.attackRange)
        {
            Owner.ChangeState(ENEMY_STATE.Attack);
            return;
        }

        Owner.agent.SetDestination(Owner.player.position);
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        Vector3 direction = Owner.player.position - Owner.transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Owner.transform.rotation = Quaternion.Slerp(
                Owner.transform.rotation,
                targetRotation,
                Time.deltaTime * Owner.RotationSpeed
            );
        }
    }

    public override void OnExitState()
    {
        Owner.agent.ResetPath();
    }
    public override void OnFixedUpdateState() { }
}
