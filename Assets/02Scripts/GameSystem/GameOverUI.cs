using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Transform gameOverPanel;

    private void Awake()
    {
        gameOverPanel.localScale = Vector3.zero;
        PlayerStatManager.OnDiePlayer += ShowGameOver;
        DungeonTimer.OnTimeOut += ShowGameOver;
    }

    private void ShowGameOver()
    {
        gameOverPanel.localScale = Vector3.one;
        Time.timeScale = 0f;
        PlayerStatManager.OnDiePlayer -= ShowGameOver;
        DungeonTimer.OnTimeOut -= ShowGameOver;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Intro");
    }
}
