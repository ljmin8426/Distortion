using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private TextMeshProUGUI bossHP;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider delaySlider;

    [SerializeField] private float lerpSpeed = 2f;
    [SerializeField] private float hideDuration = 0.5f;

    private BossCtrl boss;
    private Coroutine delayCoroutine;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        transform.localScale = Vector3.zero; // Ã³À½¿¡´Â ¼û±è
    }

    private void OnDisable()
    {
        if (boss != null)
        {
            boss.OnBossHpChanged -= UpdateHP;
            boss.OnBossDie -= HideUI;
            boss.OnBossNameChanged -= ChangeName;
        }
    }

    public void ShowUI(BossCtrl newBoss)
    {
        boss = newBoss;

        if (boss == null) return;

        boss.OnBossHpChanged += UpdateHP;
        boss.OnBossDie += HideUI;
        boss.OnBossNameChanged += ChangeName;

        hpSlider.value = 1f;
        delaySlider.value = 1f;

        transform.localScale = Vector3.one; // ÄÆ¾À ³¡³­ ½ÃÁ¡¿¡ µîÀå
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

    private void HideUI(BossCtrl boss)
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

    private void ChangeName(string name)
    {
        bossName.text = name;
    }
}
