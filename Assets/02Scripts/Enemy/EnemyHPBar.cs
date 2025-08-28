using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;      // 빨간 HP (실제)
    [SerializeField] private Slider delaySlider;   // 흰색 HP (잔상)
    [SerializeField] private float lerpSpeed = 2f; // 잔상 줄어드는 속도

    private Camera mainCamera;
    private float targetValue;

    private void Awake()
    {
        if (hpSlider == null || delaySlider == null)
        {
            Debug.LogError("hpSlider / delaySlider 할당 필요!");
        }
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        // HP바가 항상 카메라를 바라보도록
        transform.forward = mainCamera.transform.forward;

        // 흰색 잔상바가 빨간바를 천천히 따라오도록
        if (delaySlider.value > hpSlider.value)
        {
            delaySlider.value = Mathf.Lerp(
                delaySlider.value,
                hpSlider.value,
                Time.deltaTime * lerpSpeed
            );
        }
        else
        {
            delaySlider.value = hpSlider.value; // 체력 회복 시 즉시 맞춰줌
        }
    }

    /// <summary>
    /// HP 업데이트 (0~1로 정규화된 값 전달)
    /// </summary>
    public void UpdateHPBar(float normalizedHp)
    {
        normalizedHp = Mathf.Clamp01(normalizedHp);

        if (hpSlider != null)
        {
            hpSlider.value = normalizedHp; // 빨간바는 즉시 반영
        }
        // delaySlider는 LateUpdate에서 천천히 따라감
    }
}
