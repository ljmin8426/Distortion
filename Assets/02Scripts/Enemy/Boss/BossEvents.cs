using UnityEngine;

public class BossEvents : MonoBehaviour
{
    private BossController bossCtrl;

    private void Awake()
    {
        bossCtrl = GetComponentInParent<BossController>();
    }

    public void StartFight()
    {
        bossCtrl.StartFight();
    }

    public void Phase1Attack()
    {
        bossCtrl.FireConeMissiles();
    }
}
