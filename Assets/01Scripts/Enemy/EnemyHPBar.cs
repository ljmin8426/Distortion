using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    private Camera mainCamera;

    private void Awake()
    {
        if (hpSlider == null)
        {
            Debug.LogError("hpSlider 할당 필요!");
        }
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        // 카메라를 바라보도록 회전 설정
        transform.forward = mainCamera.transform.forward;
    }

    public void UpdateHPBar(float normalizedHp)
    {
        if (hpSlider != null)
        {
            hpSlider.value = Mathf.Clamp01(normalizedHp);
        }
    }
}
