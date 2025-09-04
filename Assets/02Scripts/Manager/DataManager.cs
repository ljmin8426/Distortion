using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private bool loadData = false;
    public bool LoadData => loadData;
    private GameData_SO originTable;

    private Dictionary<int, ItemData> dicItemData = new Dictionary<int, ItemData>();
    private Dictionary<int, MonsterData> dicMonsterData = new Dictionary<int, MonsterData>();

    protected override void Awake()
    {
        base.Awake();
        if(!loadData)
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

            loadData = true;
        }
    }

    public bool GetItemData(int keyID, out ItemData itemData)
    {
        return dicItemData.TryGetValue(keyID, out itemData);
    }

    public bool GetMonsterData(int ID, out MonsterData monsterData)
    {
        return dicMonsterData.TryGetValue(ID, out monsterData);
    }
}
