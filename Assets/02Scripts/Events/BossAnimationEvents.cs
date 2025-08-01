using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    public void FootStep()
    {
        AudioManager.Instance.PlaySFX("BossStep");
    }
}
