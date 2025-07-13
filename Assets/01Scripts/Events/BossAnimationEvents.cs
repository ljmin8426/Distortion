using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    public void FootStep()
    {
        AudioManager.instance.PlaySFX("BossStep");
    }
}
