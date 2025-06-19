using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    [Header("EP")]
    [SerializeField] private Slider epSlider;
    [SerializeField] private TMP_Text epText;

    [Header("±‚≈∏ Ω∫≈»")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text attackPowerText;
    [SerializeField] private TMP_Text moveSpeedText;

    private void OnEnable()
    {
        PlayerStatManager.OnHpChange += UpdateHPUI;
        PlayerStatManager.OnEpChange += UpdateEPUI;
        PlayerStatManager.OnChangeLevel += UpdateLevelUI;
        PlayerStatManager.OnChangeAttackPower += UpdateAttackPowerUI;
        PlayerStatManager.OnChangeMoveSpeed += UpdateMoveSpeedUI;
    }

    private void OnDisable()
    {
        PlayerStatManager.OnHpChange -= UpdateHPUI;
        PlayerStatManager.OnEpChange -= UpdateEPUI;
        PlayerStatManager.OnChangeLevel -= UpdateLevelUI;
        PlayerStatManager.OnChangeAttackPower -= UpdateAttackPowerUI;
        PlayerStatManager.OnChangeMoveSpeed -= UpdateMoveSpeedUI;
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

    private void UpdateAttackPowerUI(float atk)
    {
        attackPowerText.text = $"ATK: {atk:F1}";
    }

    private void UpdateMoveSpeedUI(float speed)
    {
        moveSpeedText.text = $"SPD: {speed:F1}";
    }
}
