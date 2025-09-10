using UnityEngine;

public class HitEffect : PoolObject
{
    [SerializeField] private float lifeTime = 2f; // 몇 초 뒤 풀로 반환할지

    private ParticleSystem ps;
    private float timer;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public override void OnSpawn()
    {
        timer = 0f;

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
