using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamaged
{
    public EnemyData stats;

    private Transform player;
    private NavMeshAgent agent;
    private ENEMY_STATE currentState = ENEMY_STATE.Idle;

    private float lastAttackTime;
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.moveSpeed;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

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
                    agent.SetDestination(player.position);
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
                agent.ResetPath();
                break;

            case ENEMY_STATE.Chase:
                agent.isStopped = false;
                break;

            case ENEMY_STATE.Attack:
                agent.isStopped = true;
                break;
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= stats.attackCooldown)
        {
            lastAttackTime = Time.time;
            // 애니메이션 재생 또는 이벤트 기반 공격
            Debug.Log($"{stats.enemyName}이(가) 공격합니다!");
        }
    }

    private void Die()
    {
        Debug.Log("적 사망");
        ExpManager.instance.GetExp(100);
        // 사망 애니메이션, 제거 등 처리
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"적이 {amount}의 피해를 입음! 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
