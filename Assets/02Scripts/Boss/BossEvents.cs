using UnityEngine;

public class BossEvents : MonoBehaviour
{
    private BossController bossCtrl;

    private void Awake()
    {
        bossCtrl = GetComponent<BossController>();
    }

    public void StartFight()
    {
        bossCtrl.StartFight();
    }
}
