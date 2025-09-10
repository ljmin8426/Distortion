using System.Collections;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("º¸½º ¹× UI Prefab")]
    [SerializeField] private BossCtrl bossPrefab;
    [SerializeField] private Transform point;
    [SerializeField] private BossBar bossBar;


    private void Start()
    {
        StartCoroutine(TestBoss());
    }


    private IEnumerator TestBoss()
    {
        yield return new WaitForSeconds(2f);
        BossCtrl newBoss = Instantiate(bossPrefab, point.position, Quaternion.identity);

        bossBar.Initialize(newBoss);
    }
}
