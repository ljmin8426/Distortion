using System.Collections.Generic;
using UnityEngine;

public enum BossId
{
    BossToilet = 10000,
}


public class DataManager : Singleton<DataManager>
{
    public bool LoadData { get; private set; }

    private GameData_SO originTable;

    private Dictionary<int, ItemData> dicItemData = new Dictionary<int, ItemData>();
    private Dictionary<int, MonsterData> dicMonsterData = new Dictionary<int, MonsterData>();
    private Dictionary<int, BossData> dicBossData = new Dictionary<int, BossData>();

    protected override void Awake()
    {
        base.Awake();
        if(!LoadData)
        {
            originTable = Resources.Load<GameData_SO>("GameData");

            for(int i = 0; i < originTable.weaponData.Count; i++)
            {
                dicItemData.Add(originTable.weaponData[i].itemId, originTable.weaponData[i]);
            }

            for (int i = 0; i < originTable.monsterData.Count; i++)
            {
                dicMonsterData.Add(originTable.monsterData[i].monsterId, originTable.monsterData[i]);
            }

            for (int i = 0; i < originTable.bossData.Count; i++)
            {
                dicBossData.Add(originTable.bossData[i].bossId, originTable.bossData[i]);
            }

            LoadData = true;
        }
    }

    public bool GetItemData(int keyId, out ItemData itemData)
    {
        return dicItemData.TryGetValue(keyId, out itemData);
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
