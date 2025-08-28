using UnityEngine;

public class EnemyChaseState : BaseState<EnemyBase>
{
    public EnemyChaseState(EnemyBase enemy) : base(enemy) { }

    public override void OnEnterState()
    {
        controller.Agent.isStopped = false;
        LookAtTarget();
    }

    public override void OnUpdateState()
    {
        float distance = Vector3.Distance(controller.transform.position, controller.Player.position);

        if (distance <= controller.EnemyData.attackRange)
        {
            controller.ChangeState(ENEMY_STATE.Attack);
            return;
        }

        controller.Agent.SetDestination(controller.Player.position);
    }

    private void LookAtTarget()
    {
        Vector3 direction = controller.Player.position - controller.transform.position;
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
        controller.Agent.ResetPath();
    }
    public override void OnFixedUpdateState() { }
}
