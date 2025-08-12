using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    [Header("EP")]
    [SerializeField] private Slider epSlider;
    [SerializeField] private TMP_Text epText;

    [Header("Exp")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text levelText;

    private void OnEnable()
    {
        PlayerStatManager.OnHpChange += UpdateHPUI;
        PlayerStatManager.OnEpChange += UpdateEPUI;
        PlayerStatManager.OnLevelChange += UpdateLevelUI;
        PlayerStatManager.OnChangeExp += UpdateExpUI;
    }

    private void OnDisable()
    {
        PlayerStatManager.OnHpChange -= UpdateHPUI;
        PlayerStatManager.OnEpChange -= UpdateEPUI;
        PlayerStatManager.OnLevelChange -= UpdateLevelUI;
        PlayerStatManager.OnChangeExp -= UpdateExpUI;
    }

    private void UpdateExpUI(float value, float maxValue)
    {
        expSlider.maxValue = maxValue;
        expSlider.value = value;
        expText.text = $"{(int)value} / { (int)maxValue}";
    }

    private void UpdateHPUI(float currentHP, float maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        hpText.text = $"{(int)currentHP} / {(int)maxHP}";
    }

    private void UpdateEPUI(float currentEP, float maxEP)
    {
        epSlider.maxValue = maxEP;
        epSlider.value = currentEP;
        epText.text = $"{(int)currentEP} / {(int)maxEP}";
    }

    private void UpdateLevelUI(int level)
    {
        levelText.text = $"Lv. {level}";
    }
}
