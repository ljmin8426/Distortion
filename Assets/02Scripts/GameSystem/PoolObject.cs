using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    private string poolTag;
    public string PoolTag => poolTag;

    public void SetTag(string value) => poolTag = value;

    // 오브젝트가 풀에서 꺼내질 때 실행할 동작 (Bullet이면 초기화 같은 것)
    public virtual void OnSpawn() { }

    // 오브젝트가 풀로 반환될 때 실행할 동작 (상속받는 쪽에서 구현 가능)
    public virtual void OnDespawn() { }

    // 풀로 반환
    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(this);
    }
}
