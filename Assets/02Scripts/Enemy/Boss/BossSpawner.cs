using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("º¸½º ¹× UI Prefab")]
    [SerializeField] private BossController bossPrefab;
    [SerializeField] private Transform point;
    [SerializeField] private BossBar bossBar;


    private void Start()
    {
        StartCoroutine(TestBoss());
    }


    private IEnumerator TestBoss()
    {
        yield return null;
        BossController newBoss = Instantiate(bossPrefab, point.position, point.rotation);
        bossBar.Initialize(newBoss);
    }
}
