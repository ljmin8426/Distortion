using UnityEngine;
using UnityEngine.Playables;
using System;

public class CutScene : MonoBehaviour
{
    [Header("PlayableDirector")]
    [SerializeField] private PlayableDirector cutsceneDirector;

    [Header("Option")]
    [SerializeField] private bool playOnce = true;

    [SerializeField] private GameObject gate;

    private bool hasPlayed = false;

    private PlayerInputManager playerInputManager;

    public event Action OnCutsceneFinishedEvent;

    private void OnCutsceneFinishedHandler(PlayableDirector director)
    {
        if (playerInputManager != null)
        {
            playerInputManager.enabled = true;
        }

        cutsceneDirector.stopped -= OnCutsceneFinishedHandler;

        OnCutsceneFinishedEvent?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.CompareTag("Player"))
        {
            playerInputManager = other.GetComponent<PlayerInputManager>();
            if (playerInputManager != null)
            {
                playerInputManager.enabled = true;
            }

            gate.SetActive(false);

            cutsceneDirector.Play();
            cutsceneDirector.stopped += OnCutsceneFinishedHandler;

            hasPlayed = true;
        }
    }
}
