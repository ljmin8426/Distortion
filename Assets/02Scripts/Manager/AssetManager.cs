using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
    [Header("스킬 아이템")]
    public GameObject[] skillItemPrefabs;

    [Header("장착 아이템")]
    public GameObject[] equipItemPrefabs;

    [Header("소모품 아이템")]
    public GameObject[] consumableItemPrefabs;

    [Header("아이템 드랍 확률 (%)")]
    [Range(0, 100)] public float dropNothingChance = 20f;
    [Range(0, 100)] public float skillItemChance = 30f;
    [Range(0, 100)] public float equipItemChance = 30f;
    [Range(0, 100)] public float consumableItemChance = 20f;

    private Dictionary<string, GameObject> itemDict = new();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        RegisterItems(skillItemPrefabs);
        RegisterItems(equipItemPrefabs);
        RegisterItems(consumableItemPrefabs);
    }

    private void RegisterItems(GameObject[] prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab != null)
                itemDict[prefab.name] = prefab;
        }
    }

    public GameObject GetItemPrefab(string name)
    {
        if (itemDict.TryGetValue(name, out var prefab))
            return prefab;

        Debug.LogWarning($"[AssetManager] '{name}' 프리팹을 찾을 수 없습니다.");
        return null;
    }

    public GameObject GetRandomSkillItem() => GetRandomFrom(skillItemPrefabs);
    public GameObject GetRandomEquipItem() => GetRandomFrom(equipItemPrefabs);
    public GameObject GetRandomConsumableItem() => GetRandomFrom(consumableItemPrefabs);

    /// <summary>
    /// 확률에 따라 랜덤 아이템을 반환하거나 null (드랍 없음) 반환
    /// </summary>
    public GameObject GetRandomDropItem()
    {
        float rand = Random.Range(0f, 100f);

        if (rand < dropNothingChance)
            return null;

        rand -= dropNothingChance;

        if (rand < skillItemChance)
            return GetRandomSkillItem();

        rand -= skillItemChance;

        if (rand < equipItemChance)
            return GetRandomEquipItem();

        rand -= equipItemChance;

        if (rand < consumableItemChance)
            return GetRandomConsumableItem();

        return null; // 혹시 확률 총합이 100 미만인 경우 대비
    }

    private GameObject GetRandomFrom(GameObject[] list)
    {
        if (list == null || list.Length == 0)
            return null;

        return list[Random.Range(0, list.Length)];
    }
}
