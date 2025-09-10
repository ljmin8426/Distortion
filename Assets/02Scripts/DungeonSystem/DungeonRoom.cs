using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public event Action<DungeonRoom> OnRoomCleared;

    [SerializeField] private ColliderTrigger colliderTrigger;
    [SerializeField] private SpawnPoint[] enemySpawnPoints; // 변경됨!
    [SerializeField] private GameObject entryDoor;
    [SerializeField] private GameObject exitDoor;

    private bool isActive = false;
    private bool isCleared = false;
    private List<MonsterBase> aliveEnemies = new List<MonsterBase>();

    public void ActivateRoom()
    {
        if (isCleared) return;

        isActive = true;
        SpawnEnemies();
        colliderTrigger.OnPlayerEnterTrigger += OnPlayerEnter;
    }

    private void OnPlayerEnter(object sender, EventArgs e)
    {
        if (!isCleared && isActive)
        {
            Debug.Log("플레이어가 방에 들어옴, 문 닫힘!");
            if (entryDoor != null) entryDoor.SetActive(true);
            colliderTrigger.OnPlayerEnterTrigger -= OnPlayerEnter;
        }
    }

    private void SpawnEnemies()
    {
        foreach (SpawnPoint spawnPoint in enemySpawnPoints)
        {
            // 각 스폰포인트에서 원하는 몬스터 이름으로 소환
            PoolObject obj = PoolManager.Instance.SpawnFromPool(
                spawnPoint.Monster_Type.ToString(),
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );

            obj.OnSpawn();

            MonsterBase monster = obj.GetComponent<MonsterBase>();
            if (monster != null)
            {
                aliveEnemies.Add(monster);
                monster.InitMonster();
                monster.OnMonsterDie += HandleMonsterDie;
            }
        }
    }

    private void HandleMonsterDie(MonsterBase monster)
    {
        monster.OnMonsterDie -= HandleMonsterDie;
        aliveEnemies.Remove(monster);

        if (aliveEnemies.Count == 0)
        {
            EndBattle();
        }
    }

    private void EndBattle()
    {
        Debug.Log(name + " Cleared!");
        isCleared = true;

        if (exitDoor != null) exitDoor.SetActive(false);

        OnRoomCleared?.Invoke(this);
    }
}
