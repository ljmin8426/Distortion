using TMPro;
using UnityEngine;

public class StatPanelView : MonoBehaviour
{
    public StatElement hpStat;
    public StatElement epStat;
    public StatElement attackStat;
    public StatElement speedStat;

    public void Bind(PlayerStatManager statManager)
    {
        // 첫 값 설정
        hpStat.Set("HP", $"{statManager.CurrentHP} / {statManager.MaxHP}");
        epStat.Set("EP", $"{statManager.CurrentEP} / {statManager.MaxEP}");
        attackStat.Set("ATK", statManager.ATK.ToString());

        // 이벤트 구독
        PlayerStatManager.OnHpChange += (cur, max) =>
            hpStat.Set("HP", $"{cur} / {max}");
        PlayerStatManager.OnEpChange += (cur, max) =>
            epStat.Set("EP", $"{cur} / {max}");
        PlayerStatManager.OnAttackPowerChange += (atk) =>
            attackStat.Set("ATK", atk.ToString());
    }
}
