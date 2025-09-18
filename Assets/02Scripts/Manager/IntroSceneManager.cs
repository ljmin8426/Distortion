using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene_Name
{
    Intro,
    Stage
}

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject pressSpaceText;

    private bool canStart = false;

    private void Start()
    {
        if (pressSpaceText != null)
            pressSpaceText.SetActive(false);

        DataManager.Instance.OnDataReady += HandleDataReady;

        if (DataManager.Instance != null && DataManager.Instance.IsReady)
            HandleDataReady();
    }

    private void OnDestroy()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.OnDataReady -= HandleDataReady;
    }

    private void HandleDataReady()
    {
        if (pressSpaceText != null)
            pressSpaceText.SetActive(true); // 준비 완료 후 UI 표시
        canStart = true;
    }

    private void Update()
    {
        if (canStart && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(Scene_Name.Stage.ToString());
        }
    }
}