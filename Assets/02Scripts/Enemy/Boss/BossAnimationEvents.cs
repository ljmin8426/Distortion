using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    private BossCtrl bossCtrl;

    private void Awake()
    {
        bossCtrl = GetComponentInParent<BossCtrl>();
    }

    public void StartFight()
    {
        bossCtrl.StartFight();
    }
}
