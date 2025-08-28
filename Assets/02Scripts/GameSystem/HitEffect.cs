using UnityEngine;

public class HitEffect : PoolObject
{
    [SerializeField] private float lifeTime = 2f; // 몇 초 뒤 풀로 반환할지
    private float timer;

    public override void OnSpawn()
    {
        timer = 0f;

        // 파티클 다시 시작
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Clear();
            ps.Play();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            ReturnToPool();
        }
    }
}
