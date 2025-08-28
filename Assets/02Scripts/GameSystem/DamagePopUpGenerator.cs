using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : SingletonDestroy<DamagePopUpGenerator>
{
    [SerializeField] private string damageTextTag; // 풀에서 꺼낼 때 태그

    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = PoolManager.Instance.SpawnFromPool(damageTextTag, position, Quaternion.identity);
        if (popup == null)
        {
            Debug.LogError($"[DamagePopUpGenerator] Failed to spawn popup with tag={damageTextTag}");
            return;
        }

        var anim = popup as DamageTextAnimation;
        if (anim != null)
        {
            anim.SetText(text, color, position);
        }
    }

}
