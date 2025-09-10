using TMPro;
using UnityEngine;

public class StatPanelView : MonoBehaviour
{
    public StatElement hpStat;
    public StatElement epStat;
    public StatElement atkStat;
    public StatElement agStat;

    public void Bind(PlayerStatManager statManager)
    {
        hpStat.Set("HP", $"{statManager.CurrentHP} / {statManager.MaxHP}");
        epStat.Set("EP", $"{statManager.CurrentEP} / {statManager.MaxEP}");
        atkStat.Set("ATK", statManager.ATK.ToString());
        agStat.Set("AG", statManager.AG.ToString());

        PlayerStatManager.OnHpChange += (cur, max) =>
            hpStat.Set("HP", $"{cur} / {max}");
        PlayerStatManager.OnEpChange += (cur, max) =>
            epStat.Set("EP", $"{cur} / {max}");
        PlayerStatManager.OnAtkChange += (atk) =>
            atkStat.Set("ATK", atk.ToString());
        PlayerStatManager.OnAgChange += (ag) =>
            agStat.Set("AG", ag.ToString());
    }
}
