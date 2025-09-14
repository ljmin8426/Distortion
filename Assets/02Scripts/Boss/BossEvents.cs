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

    // 애니메이션 이벤트에서 호출
    public void OnHit()
    {
        bossCtrl.SpawnAttackRange();
    }
}
