using TMPro;
using UnityEngine;
using System;

public class DungeonTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float limitTime = 600f; // 10분

    private bool timerRunning = false;
    private float currentTime;
    private int lastDisplayedSeconds = -1;

    // 타이머 변경 이벤트 (UI가 구독해서 업데이트)
    public event Action<float> OnTimeChanged;

    private void OnEnable()
    {
        DungeonDoor.OnDoorOpened += StartTimer;
        OnTimeChanged += UpdateTimerUI; // UI 구독
    }

    private void OnDisable()
    {
        DungeonDoor.OnDoorOpened -= StartTimer;
        OnTimeChanged -= UpdateTimerUI;
    }

    private void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        int secondsLeft = Mathf.FloorToInt(currentTime);
        if (secondsLeft != lastDisplayedSeconds)
        {
            lastDisplayedSeconds = secondsLeft;
            OnTimeChanged?.Invoke(currentTime);
        }

        if (currentTime <= 0f)
        {
            timerRunning = false;
            Debug.Log("던전 실패! 제한 시간 초과");
            // 실패 로직 처리
        }
    }

    private void StartTimer()
    {
        currentTime = limitTime;
        timerRunning = true;
        lastDisplayedSeconds = -1; // UI 갱신 초기화
        Debug.Log("던전 시작! 제한 시간 카운트다운 시작");
    }

    public void StopTimer()
    {
        if (!timerRunning) return;

        timerRunning = false;
        Debug.Log($"던전 클리어! 남은 시간: {Mathf.FloorToInt(currentTime)}초");
    }

    private void UpdateTimerUI(float time)
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
