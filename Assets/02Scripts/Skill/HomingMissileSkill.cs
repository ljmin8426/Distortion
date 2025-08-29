using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileSkill : SkillBase
{
    [Header("Missile Settings")]
    [SerializeField] private int missileCount = 3;
    [SerializeField] private float missileSpreadAngle = 15f;
    [SerializeField] private float missileSpawnInterval = 0.1f;
    [SerializeField] private GameObject effectPrefab;

    public override void Activate(GameObject attacker)
    {
        Transform priorityTarget = FindPriorityTarget(attacker.transform.position, 20f);
        if (priorityTarget == null)
        {
            Debug.Log("타겟이 없어 스킬을 사용할 수 없습니다");
            return;
        }

        PlayerStatManager.Instance.ConsumeEP(manaCost);

        if (!TryUseSkill())
            return;

        StartCoroutine(FireMultipleMissiles(attacker));
        StartCoroutine(CooldownRoutine());
    }

    private Transform FindPriorityTarget(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        Transform bossTarget = null;
        float bossDist = float.MaxValue;

        Transform enemyTarget = null;
        float enemyDist = float.MaxValue;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Boss"))
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < bossDist)
                {
                    bossDist = dist;
                    bossTarget = col.transform;
                }
            }
            else if (col.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < enemyDist)
                {
                    enemyDist = dist;
                    enemyTarget = col.transform;
                }
            }
        }

        if (bossTarget != null)
            return bossTarget;

        return enemyTarget;
    }


    private IEnumerator FireMultipleMissiles(GameObject attacker)
    {
        List<Transform> targets = FindClosestEnemies(attacker.transform.position, missileCount);

        for (int i = 0; i < missileCount; i++)
        {
            // 발사 위치: 머리 위 + 살짝 퍼짐
            float angleOffset = (i - (missileCount - 1) / 2f) * missileSpreadAngle;
            Vector3 direction = Quaternion.Euler(0, angleOffset, 0) * attacker.transform.forward;
            Vector3 spawnPos = attacker.transform.position + Vector3.up * 1.2f + direction * 0.2f;

            GameObject missile = Instantiate(effectPrefab, spawnPos, Quaternion.LookRotation(direction));

            GameObject target = (i < targets.Count) ? targets[i].gameObject : FindNearestEnemy(attacker.transform.position);
            if (target != null)
            {
                HomingMissile missileScript = missile.GetComponent<HomingMissile>();
                if (missileScript != null)
                {
                    missileScript.Initialize(target.transform);
                }
            }

            if (missileSpawnInterval > 0f)
                yield return new WaitForSeconds(missileSpawnInterval);
        }
    }

    private List<Transform> FindClosestEnemies(Vector3 position, int count)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 20f);
        List<Transform> enemies = new List<Transform>();

        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemies.Add(col.transform);
            }
        }

        enemies.Sort((a, b) =>
            Vector3.Distance(position, a.position).CompareTo(
            Vector3.Distance(position, b.position)));

        return enemies.GetRange(0, Mathf.Min(count, enemies.Count));
    }

    private GameObject FindNearestEnemy(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 20f);
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = col.gameObject;
                }
            }
        }

        return nearest;
    }
}
