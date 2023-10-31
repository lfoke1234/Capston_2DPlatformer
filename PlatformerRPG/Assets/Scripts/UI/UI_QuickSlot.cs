using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickSlot : UI_ItemSlot
{
    public int quickSlotNumber;

    private void OnValidate()
    {
        gameObject.name = "Quick Slot -" + quickSlotNumber.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Useable usableItem = item.data as ItemData_Useable;

        if (usableItem != null)
        {
            Inventory.Instance.UnequipUsableItem(usableItem);
            Inventory.Instance.AddItemWithStack(usableItem, item.stackSize);
        }
    }
}
