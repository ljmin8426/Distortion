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
    private Dictionary<string, SpriteData> dicSpriteData = new Dictionary<string, SpriteData>();

    public event Action OnDataReady;

    protected override void Awake()
    {
        base.Awake();
        if(!isReady)
        {
            gameData = Resources.Load<GameData_SO>("GameData");

            SetMonsterData();
            SetSpriteData();

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

    private void SetSpriteData()
    {
        for(int i = 0; i < gameData.spriteData.Count;i++)
        {
            dicSpriteData.Add(gameData.spriteData[i].spriteName, gameData.spriteData[i]);
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

    public bool GetSprite(string keyId, out SpriteData spriteData)
    {
        return dicSpriteData.TryGetValue(keyId, out spriteData);
    }
}
