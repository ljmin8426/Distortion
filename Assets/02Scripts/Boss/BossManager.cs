using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    [SerializeField] private BossCtrl bossPrefab;
    [SerializeField] private Transform spawnPoint;
    public event Action<BossCtrl> OnBossSpawned;
    private void Awake() => Instance = this;

    public BossCtrl SpawnBoss()
    {
        BossCtrl boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        boss.InitBoss();
        OnBossSpawned?.Invoke(boss);
        return boss;
    }
}