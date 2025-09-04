using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    private string poolTag;
    public string PoolTag => poolTag;

    public void SetTag(string value) => poolTag = value;

    public virtual void OnSpawn() { }

    public virtual void OnDespawn() { }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(this);
    }
}
