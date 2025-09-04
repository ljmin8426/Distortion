using System.Collections.Generic;
using System;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private enum State
    {
        Idle,
        Active,
        Cleared
    }

    [SerializeField] private ColliderTrigger colliderTrigger;
    [SerializeField] private Wave[] waveArray;
    [SerializeField] private GameObject doorToOpen; // 전투 끝나면 열릴 문
    [SerializeField] private GameObject doorToClose; // 전투 시작할 때 닫힐 문 (입구문)

    private State state;

    private void Awake()
    {
        state = State.Idle;
    }

    private void Start()
    {
        colliderTrigger.OnPlayerEnterTrigger += OnPlayerEnter;
    }

    private void OnPlayerEnter(object sender, EventArgs e)
    {
        if (state == State.Idle)
        {
            StartBattle();
            colliderTrigger.OnPlayerEnterTrigger -= OnPlayerEnter; // 중복 발동 방지
        }
    }

    private void StartBattle()
    {
        Debug.Log("Start Battle");
        state = State.Active;

        // 입구문 닫기
        if (doorToClose != null)
        {
            doorToClose.SetActive(true);
        }
    }

    private void Update()
    {
        if (state == State.Active)
        {
            bool allWavesOver = true;
            foreach (Wave wave in waveArray)
            {
                wave.Update();
                if (!wave.IsWaveOver())
                    allWavesOver = false;
            }

            if (allWavesOver)
            {
                EndBattle();
            }
        }
    }

    private void EndBattle()
    {
        Debug.Log("Battle End!");
        state = State.Cleared;

        // 출구문 열기
        if (doorToOpen != null)
        {
            doorToOpen.SetActive(false);
        }
    }

    [System.Serializable]
    private class Wave
    {
        [SerializeField] private string waveName;
        [SerializeField] private Transform[] enemyTransformArray;
        [SerializeField] private float timer;

        private List<MonsterBase> aliveEnemies = new List<MonsterBase>();   

        public void Update()
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    SpawnEnemies();
                }
            }
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < enemyTransformArray.Length; i++)
            {
                PoolObject obj = PoolManager.Instance.SpawnFromPool("Melee", enemyTransformArray[i].position, Quaternion.identity);
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
        }

        public bool IsWaveOver()
        {
            return timer <= 0 && aliveEnemies.Count == 0;
        }
    }
}
