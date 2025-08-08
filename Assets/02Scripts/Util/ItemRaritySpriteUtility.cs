using UnityEngine;

// 에셋 번들로 변경후 삭제
public static class ItemRaritySpriteUtility
{
    public static Sprite GetBackgroundSprite(ITEM_RARITY rarity)
    {
        return rarity switch
        {
            ITEM_RARITY.Common => Resources.Load<Sprite>("ItemBGs/bg_common"),
            ITEM_RARITY.Rare => Resources.Load<Sprite>("ItemBGs/bg_rare"),
            ITEM_RARITY.Epic => Resources.Load<Sprite>("ItemBGs/bg_epic"),
            ITEM_RARITY.Legendary => Resources.Load<Sprite>("ItemBGs/bg_legendary"),
            _ => Resources.Load<Sprite>("ItemBGs/bg_default"),
        };
    }
}
