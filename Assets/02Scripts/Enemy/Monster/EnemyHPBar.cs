using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;      
    [SerializeField] private Slider delaySlider;   
    [SerializeField] private float lerpSpeed = 2f; 

    private Camera mainCamera;

    private void Awake()
    {
        if (hpSlider == null || delaySlider == null)
        {
            Debug.LogError("hpSlider / delaySlider 할당 필요");
        }
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        transform.forward = mainCamera.transform.forward;

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
            delaySlider.value = hpSlider.value; 
        }
    }

    public void UpdateHPBar(float normalizedHp)
    {
        normalizedHp = Mathf.Clamp01(normalizedHp);

        if (hpSlider != null)
        {
            hpSlider.value = normalizedHp;
        }
    }
}
