using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;                // key 값
        public GameObject prefab;         // 실제 생성될 오브젝트
        public int size;                  // 초기 생성 개수
        public Transform parentTransform; // 부모 오브젝트
    }

    [Header("Pool Info")]
    [SerializeField] private List<Pool> pools = new List<Pool>();

    private Dictionary<string, List<PoolObject>> poolDictionary;

    protected override void Awake()
    {
        base.Awake();

        poolDictionary = new Dictionary<string, List<PoolObject>>();

        foreach (Pool pool in pools)
        {
            if (!poolDictionary.ContainsKey(pool.tag))
                poolDictionary.Add(pool.tag, new List<PoolObject>());

            AddPoolObject(pool.tag, pool.size);
        }
    }

    private void AddPoolObject(string tag, int count = 1)
    {
        Pool pool = pools.Find(obj => obj.tag == tag);
        if (pool == null)
        {
            Debug.LogWarning($"[PoolManager] {tag} 풀을 찾을 수 없습니다.");
            return;
        }

        Transform parent = pool.parentTransform != null ? pool.parentTransform : transform;

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(pool.prefab, parent);
            go.SetActive(false);

            PoolObject poolObj = go.GetComponent<PoolObject>();
            if (poolObj == null)
                poolObj = go.AddComponent<PoolObject>();

            poolObj.SetTag(tag);
            poolDictionary[tag].Add(poolObj);
        }
    }

    public PoolObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[PoolManager] {tag} 풀은 존재하지 않습니다.");
            return null;
        }

        PoolObject poolObj = null;

        foreach (var obj in poolDictionary[tag])
        {
            if (!obj.gameObject.activeSelf)
            {
                poolObj = obj;
                break;
            }
        }

        if (poolObj == null)
        {
            AddPoolObject(tag);
            poolObj = poolDictionary[tag][poolDictionary[tag].Count - 1];
        }

        poolObj.transform.SetPositionAndRotation(position, rotation);
        poolObj.gameObject.SetActive(true);
        poolObj.OnSpawn();

        return poolObj;
    }

    public void ReturnToPool(PoolObject obj)
    {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
    }
}
