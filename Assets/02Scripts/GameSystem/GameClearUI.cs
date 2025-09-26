using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private float timeToRobby = 5f;

    private void Awake()
    {
        gameClearUI.transform.localScale = Vector3.zero;
        DungeonManager.OnAllClear += Initialize;
    }

    private void OnDisable()
    {
        DungeonManager.OnAllClear -= Initialize;
    }

    public void Initialize(BossController newBoss)
    {
        newBoss.OnBossDie += ShowGameClear;
    }

    private void ShowGameClear(BossController newBoss)
    {
        StartCoroutine(ReturnToLobbyAfterDelay());
    }

    private IEnumerator ReturnToLobbyAfterDelay()
    {
        yield return YieldCache.WaitForSeconds(2f);
        gameClearUI.transform.localScale = Vector3.one;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(timeToRobby);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Intro");
    }
}
