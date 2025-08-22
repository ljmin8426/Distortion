using UnityEngine;

public class EnemyChaseState : BaseState<EnemyBase>
{
    public EnemyChaseState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.agent.isStopped = false;
        LookAtTarget();
    }

    public override void OnUpdateState()
    {
        float distance = Vector3.Distance(controller.transform.position, controller.player.position);

        if (distance <= controller.EnemyData.attackRange)
        {
            controller.ChangeState(ENEMY_STATE.Attack);
            return;
        }

        controller.agent.SetDestination(controller.player.position);
    }

    private void LookAtTarget()
    {
        Vector3 direction = controller.player.position - controller.transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.transform.rotation = Quaternion.Slerp(
                controller.transform.rotation,
                targetRotation,
                Time.deltaTime * 10
            );
        }
    }


    public override void OnExitState()
    {
        controller.agent.ResetPath();
    }
    public override void OnFixedUpdateState() { }
}
