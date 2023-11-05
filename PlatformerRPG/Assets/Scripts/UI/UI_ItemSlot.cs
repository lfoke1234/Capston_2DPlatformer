using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Cinemachine.Utility;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
            ui.itemTooltip.HideToolTip();
        }

        else if (item.data.itemType == ItemType.Useable)
        {
            Inventory.Instance.EquipUsableItem(item.data);
            ui.itemTooltip.HideToolTip();
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePosition.y > 320)
            yOffset = -150;
        else
            yOffset = 150;

        ui.itemTooltip.ShowToolTip(item.data);
        ui.itemTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemTooltip.HideToolTip();
    }
}
