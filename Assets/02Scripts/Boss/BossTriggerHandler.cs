using UnityEngine;

public class BossTriggerHandler : MonoBehaviour
{
    [SerializeField] private CutScene cutscene;
    [SerializeField] private BossCtrl boss; // 컷씬 전에 이미 씬에 존재하거나 생성됨
    [SerializeField] private BossBar bossBar;

    private void OnEnable()
    {
        if (cutscene != null)
            cutscene.OnCutsceneFinishedEvent += OnCutsceneEnd;
    }

    private void OnDisable()
    {
        if (cutscene != null)
            cutscene.OnCutsceneFinishedEvent -= OnCutsceneEnd;
    }

    private void OnCutsceneEnd()
    {
        // 컷씬 종료 시점에 체력바 등장
        boss = FindAnyObjectByType<BossCtrl>();
        bossBar.ShowUI(boss);
    }
}
