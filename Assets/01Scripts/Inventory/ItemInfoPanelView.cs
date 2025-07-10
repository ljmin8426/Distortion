using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanelView : MonoBehaviour
{
    public Image itemImage;
    public Sprite EmptyImage;
    public List<StatElement> statElements;

    public void ShowItemInfo(ItemSO item)
    {
        if (item == null)
        {
            ClearAllStatElements();
            itemImage.sprite = null;
            return;
        }

        itemImage.sprite = item.icon;

        if (item is EquipmentItemT equip && equip.statList != null)
        {
            for (int i = 0; i < statElements.Count; i++)
            {
                if (i < equip.statList.Count)
                {
                    var stat = equip.statList[i];
                    if (statElements[i] != null)
                    {
                        statElements[i].Set(stat.statType.ToString(), stat.value.ToString());
                        statElements[i].gameObject.SetActive(true);
                    }
                }
                else if (statElements[i] != null)
                {
                    statElements[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            ClearAllStatElements();
        }
    }

    private void ClearAllStatElements()
    {
        foreach (var statElement in statElements)
        {
            if (statElement != null)
            {
                statElement.gameObject.SetActive(false);
            }
        }
    }
    public void Hide()
    {
        itemImage.sprite = EmptyImage;

        // 스탯 패널 모두 비활성화
        foreach (var stat in statElements)
        {
            if (stat != null)
                stat.gameObject.SetActive(false);
        }
    }
}
