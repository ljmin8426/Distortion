using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField] private Slider dashSlider;
    [SerializeField] private float fillSpeed = 5f; // 부드럽게 채워지는 속도

    private int targetValue;

    private void OnEnable()
    {
        PlayerStatManager.OnDashChange += OnDashChanged;
    }

    private void OnDisable()
    {
        PlayerStatManager.OnDashChange -= OnDashChanged;
    }

    private void Start()
    {
        dashSlider.maxValue = PlayerStatManager.Instance.MaxDash;
        dashSlider.value = PlayerStatManager.Instance.DashAmount;
    }

    private void OnDashChanged(int newValue)
    {
        targetValue = newValue;
        StopAllCoroutines();
        StartCoroutine(SmoothUpdate());
    }

    private IEnumerator SmoothUpdate()
    {
        while (!Mathf.Approximately(dashSlider.value, targetValue))
        {
            dashSlider.value = Mathf.Lerp(dashSlider.value, targetValue, Time.deltaTime * fillSpeed);
            yield return null;
        }
        dashSlider.value = targetValue;
    }
}
