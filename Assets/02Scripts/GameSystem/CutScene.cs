using UnityEngine;
using UnityEngine.Playables;
using System;

public class CutScene : MonoBehaviour
{
    [Header("PlayableDirector")]
    public PlayableDirector cutsceneDirector;

    [Header("Option")]
    public bool playOnce = true;
    private bool hasPlayed = false;

    private PlayerCtrl playerController;
    public BossCutScene bossCutScene; // 추가

    public event Action OnCutsceneFinishedEvent;

    private void OnCutsceneFinishedHandler(PlayableDirector director)
    {
        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.SetMove(true);
        }

        cutsceneDirector.stopped -= OnCutsceneFinishedHandler;

        OnCutsceneFinishedEvent?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerCtrl>();
            if (playerController != null)
            {
                playerController.enabled = false;
                playerController.SetMove(false);
            }

            // 보스 찾기 & 카메라 설정
            bossCutScene?.FindBoss();

            cutsceneDirector.Play();
            cutsceneDirector.stopped += OnCutsceneFinishedHandler;

            hasPlayed = true;
        }
    }
}
