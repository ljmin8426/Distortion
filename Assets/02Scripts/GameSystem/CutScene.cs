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

            cutsceneDirector.Play();
            cutsceneDirector.stopped += OnCutsceneFinishedHandler;

            hasPlayed = true;
        }
    }
}
