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

    private PlayerController playerController;

    public event Action OnCutsceneFinishedEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            BossManager.Instance.SpawnBoss();

            playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
                playerController.StopMove();
            }

            cutsceneDirector.Play();
            cutsceneDirector.stopped += OnCutsceneFinishedHandler;

            hasPlayed = true;
        }
    }

    private void OnCutsceneFinishedHandler(PlayableDirector director)
    {
        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.StopMove();
        }

        cutsceneDirector.stopped -= OnCutsceneFinishedHandler;

        OnCutsceneFinishedEvent?.Invoke();
    }
}
