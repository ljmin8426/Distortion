using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite selectHighlight;

    public ItemDataSO Item { get; private set; }

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Set(ItemDataSO item)
    {
        Item = item;
        itemImage.sprite = item.icon;
        itemImage.enabled = true;

        Sprite sprite = backgroundImage.sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryEvents.OnItemClick?.Invoke(Item, this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item.itemType == Item_Type.Consumable)
            {
                PlayerStatManager stat = PlayerStatManager.Instance;
                if (Item is PotionItem potion)
                {
                    if (potion.restoreHP)
                        stat.RecoverHP(potion.restoreAmount);
                    else
                        stat.RecoverEP(potion.restoreAmount);
                }

                InventoryEvents.OnUseItem?.Invoke(Item);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }
}
