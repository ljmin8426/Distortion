using System;
using System.Collections.Generic;
using UnityEngine;

public enum BossId
{
    BossToilet = 10000,
}


public class DataManager : Singleton<DataManager>
{
    private bool isReady = false;
    public bool IsReady => isReady;

    private MonsterData_SO monsterData;

    private Dictionary<int, MonsterData> dicMonsterData = new Dictionary<int, MonsterData>();
    private Dictionary<int, BossData> dicBossData = new Dictionary<int, BossData>();

    public event Action OnDataReady;

    protected override void Awake()
    {
        base.Awake();
        if(!isReady)
        {
            monsterData = Resources.Load<MonsterData_SO>("MonsterData");

            for (int i = 0; i < monsterData.monsterData.Count; i++)
            {
                dicMonsterData.Add(monsterData.monsterData[i].monsterId, monsterData.monsterData[i]);
            }

            for (int i = 0; i < monsterData.bossData.Count; i++)
            {
                dicBossData.Add(monsterData.bossData[i].bossId, monsterData.bossData[i]);
            }

            isReady = true;

            OnDataReady?.Invoke();
        }
    }

    public bool GetMonsterData(int keyId, out MonsterData monsterData)
    {
        return dicMonsterData.TryGetValue(keyId, out monsterData);
    }

    public bool GetBossData(int keyId, out BossData bossData)
    {
        return dicBossData.TryGetValue(keyId, out bossData);
    }
}
