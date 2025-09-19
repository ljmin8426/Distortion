using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Potion Item")]
public class PotionItem : ItemDataSO
{
    public bool restoreHP;
    public float restoreAmount;
}
