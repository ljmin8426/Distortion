using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamaged
{
    [SerializeField] private EnemyData stats;

    private Transform targetPlayer;
    private NavMeshAgent navAgent;

    private ENEMY_STATE currentState = ENEMY_STATE.Idle;

    private float lastAttackTime;
    private int currentHealth;


    private void Awake()
    {
        currentHealth = stats.maxHP;
    }

    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player")?.transform;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = stats.moveSpeed;
        navAgent.updateRotation = false;
    }

    void Update()
    {
        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        switch (currentState)
        {
            case ENEMY_STATE.Idle:
                if (distance <= stats.detectionRange)
                    ChangeState(ENEMY_STATE.Chase);
                break;

            case ENEMY_STATE.Chase:
                if (distance <= stats.attackRange)
                {
                    ChangeState(ENEMY_STATE.Attack);
                }
                else if (distance > stats.chaseStopRange)
                {
                    ChangeState(ENEMY_STATE.Idle);
                }
                else
                {
                    navAgent.SetDestination(targetPlayer.position);
                }
                break;

            case ENEMY_STATE.Attack:
                if (distance > stats.attackRange)
                {
                    ChangeState(ENEMY_STATE.Chase);
                }
                else
                {
                    TryAttack();
                }
                break;
        }
    }

    void ChangeState(ENEMY_STATE newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case ENEMY_STATE.Idle:
                navAgent.ResetPath();
                break;

            case ENEMY_STATE.Chase:
                navAgent.isStopped = false;
                break;

            case ENEMY_STATE.Attack:
                navAgent.isStopped = true;
                break;
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= stats.attackCooldown)
        {
            lastAttackTime = Time.time;
            // 이벤트 기반 공격
        }
    }

    private void Die()
    {
        Debug.Log("적 사망");
        ExpManager.instance.GetExp(100);
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"적이 {amount}의 피해를 입음 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
