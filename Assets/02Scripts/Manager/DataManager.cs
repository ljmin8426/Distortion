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

    private GameData_SO gameData;

    private Dictionary<int, MonsterData> dicMonsterData = new Dictionary<int, MonsterData>();
    private Dictionary<int, BossData> dicBossData = new Dictionary<int, BossData>();

    public event Action OnDataReady;

    protected override void Awake()
    {
        base.Awake();
        if(!isReady)
        {
            gameData = Resources.Load<GameData_SO>("GameData");

            SetMonsterData();

            isReady = true;
            OnDataReady?.Invoke();
        }
    }

    private void SetMonsterData()
    {
        for (int i = 0; i < gameData.monsterData.Count; i++)
        {
            dicMonsterData.Add(gameData.monsterData[i].monsterId, gameData.monsterData[i]);
        }

        for (int i = 0; i < gameData.bossData.Count; i++)
        {
            dicBossData.Add(gameData.bossData[i].bossId, gameData.bossData[i]);
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
