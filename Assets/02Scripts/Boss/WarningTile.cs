using UnityEngine;

public class WarningTile : MonoBehaviour
{
    [Header("레이저 프리팹")]
    [SerializeField] private GameObject laserPrefab;

    [Header("지연 시간 (초)")]
    [SerializeField] private float delay = 2.0f;

    private void Start()
    {
        Invoke(nameof(SpawnLaser), delay);
    }

    private void SpawnLaser()
    {
        if (laserPrefab == null)
        {
            Debug.LogWarning("[AlertLine] laserPrefab이 설정되지 않았습니다.");
            return;
        }

        Vector3 spawnPos = transform.position;
        GameObject laser = Instantiate(laserPrefab, spawnPos, Quaternion.identity);

        Destroy(gameObject); // 경고 표시 제거
    }
}
