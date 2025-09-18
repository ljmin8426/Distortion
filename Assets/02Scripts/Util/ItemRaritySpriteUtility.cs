using UnityEngine;

public static class ItemRaritySpriteUtility
{
    public static Sprite GetBackgroundSprite(Item_Rarity rarity)
    {
        return rarity switch
        {
            Item_Rarity.Common => Resources.Load<Sprite>("ItemBGs/bg_common"),
            Item_Rarity.Rare => Resources.Load<Sprite>("ItemBGs/bg_rare"),
            Item_Rarity.Epic => Resources.Load<Sprite>("ItemBGs/bg_epic"),
            Item_Rarity.Legendary => Resources.Load<Sprite>("ItemBGs/bg_legendary"),
            _ => Resources.Load<Sprite>("ItemBGs/bg_default"),
        };
    }
}
