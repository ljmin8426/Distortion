using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [Header("TMPro")]
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private TextMeshProUGUI bossHP;

    [Header("Slider")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider delaySlider;

    [Header("Setting")]
    [SerializeField] private float lerpSpeed = 0.2f;
    [SerializeField] private float hideDuration = 0.2f;

    private BossCtrl boss;
    private Coroutine delayCoroutine;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        transform.localScale = Vector3.zero; // Ã³À½¿¡´Â ¼û±è
    }

    public void Initialize(BossCtrl newBoss)
    {
        boss = newBoss;
        boss.OnFightReady += ShowUI;
        boss.OnBossHpChanged += UpdateHP;
        boss.OnBossDie += HideUI;
    }

    private void OnDisable()
    {
        if (boss != null)
        {
            boss.OnFightReady -= ShowUI;
            boss.OnBossHpChanged -= UpdateHP;
            boss.OnBossDie -= HideUI;
        }
    }

    private void ShowUI(BossCtrl bossCtrl)
    {
        bossName.text = bossCtrl.BossData.bossName;
        hpSlider.value = 1f;
        delaySlider.value = 1f;
        transform.localScale = Vector3.one;
    }

    private void UpdateHP(float curHP, float maxHP)
    {
        float targetPercent = curHP / maxHP;
        hpSlider.value = targetPercent;
        bossHP.text = $"{(int)curHP} / {(int)maxHP}";

        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        delayCoroutine = StartCoroutine(UpdateDelayBar(targetPercent));
    }

    private IEnumerator UpdateDelayBar(float targetValue)
    {
        if (delaySlider.value > targetValue)
        {
            while (delaySlider.value > targetValue)
            {
                delaySlider.value = Mathf.MoveTowards(delaySlider.value, targetValue, Time.deltaTime * lerpSpeed);
                yield return null;
            }
        }
        else
        {
            delaySlider.value = targetValue;
        }
    }

    private void HideUI(BossCtrl bossCtrl)
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideWithScale());
    }

    private IEnumerator HideWithScale()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / hideDuration);
            yield return null;
        }

        transform.localScale = Vector3.zero;
    }
}
