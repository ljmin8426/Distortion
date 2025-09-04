using UnityEngine;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    [Header("컷씬 PlayableDirector")]
    public PlayableDirector cutsceneDirector;

    [Header("플레이어")]
    public GameObject player; // Player 오브젝트 연결
    private PlayerController playerController; // PlayerController 참조

    [Header("옵션")]
    public bool playOnce = true;
    private bool hasPlayed = false;

    private void Start()
    {
        // PlayerController 컴포넌트 가져오기 (예: PlayerMovement, PlayerController 등)
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed && playOnce) return;

        if (other.gameObject == player)
        {
            // 플레이어 움직임 막기
            if (playerController != null) playerController.enabled = false;

            // 컷씬 재생
            cutsceneDirector.Play();

            // 컷씬 종료 시 플레이어 컨트롤 복구
            cutsceneDirector.stopped += OnCutsceneFinished;

            hasPlayed = true;
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        if (playerController != null) playerController.enabled = true;

        // 이벤트 해제 (메모리 누수 방지)
        cutsceneDirector.stopped -= OnCutsceneFinished;
    }
}
