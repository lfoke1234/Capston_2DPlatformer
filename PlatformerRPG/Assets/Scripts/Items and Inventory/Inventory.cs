using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemData> startingEquipment;
    public List<ItemData> startEquipItemData;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary <ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> usable;
    public Dictionary<ItemData_Useable, InventoryItem> usableDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform EquipmentSlotParnet;
    [SerializeField] private Transform statSlotParnet;
    [SerializeField] private Transform quickSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;
    private UI_QuickSlot[] quickSlot;

    [Header("Items Cooldown")]
    private float lastTimeUsedFalsk;
    private float flaskCooldown;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        usable = new List<InventoryItem>();
        usableDictionary = new Dictionary<ItemData_Useable, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = EquipmentSlotParnet.GetComponentsInChildren<UI_EquipmentSlot>();
        quickSlot = quickSlotParent.GetComponentsInChildren<UI_QuickSlot>();

        statSlot = statSlotParnet.GetComponentsInChildren<UI_StatSlot>();

        StartingItme();
        StartEquipItem();
    }

    private void StartingItme()
    {
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            AddItem(startingEquipment[i]);
        }
    }

    private void StartEquipItem()
    {
        for (int i = 0; i < startEquipItemData.Count; i++)
        {
            EquipItem(startEquipItemData[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquiment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquiment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquiment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquiment, newItem);

        newEquiment.AddModifire();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifire();
        }
    }

    public void EquipUsableItem(ItemData _item)
    {
        ItemData_Useable newUsable = _item as ItemData_Useable;

        if (usableDictionary.TryGetValue(newUsable, out InventoryItem existingItem))
        {
            existingItem.stackSize += stashDictionary[_item].stackSize; 
        }
        else
        {
            InventoryItem newItem = new InventoryItem(newUsable);
            newItem.stackSize = stashDictionary[_item].stackSize; 
            usable.Add(newItem);
            usableDictionary.Add(newUsable, newItem);
        }

        RemoveItem(_item);
        UpdateSlotUI();
    }

    public void AddItemWithStack(ItemData _item, int stackSizeToAdd)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem existingItem))
        {
            existingItem.stackSize += stackSizeToAdd;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = stackSizeToAdd;
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }

        UpdateSlotUI();
    }

    public void UnequipUsableItem(ItemData_Useable itemToRemove)
    {
        if (usableDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            usable.Remove(value);
            usableDictionary.Remove(itemToRemove);
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < quickSlot.Length; i++)
        {
            quickSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < usable.Count; i++)
        {
            quickSlot[i].UpdateSlot(usable[i]);
        }

        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddtoInventory())
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material || _item.itemType == ItemType.Useable)
        {
            AddStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }


        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
        }


        UpdateSlotUI();
    }

    public bool CanAddtoInventory()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        }

        return true;
    }

    public bool CanCreaft(ItemData_Equipment _itemToCreaft, List<InventoryItem> _requiredMaterial)
    {
        List<InventoryItem> materialToRemove = new List<InventoryItem>();


        for (int i = 0; i < _requiredMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterial[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterial[i].stackSize)
                {
                    Debug.Log("You dont have Materials");
                    return false;
                }
                else
                {
                    materialToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("You dont have Materials");
                return false;
            }
        }

        for (int i = 0; i < materialToRemove.Count; i++)
        {
            RemoveItem(materialToRemove[i].data);
        }

        AddItem(_itemToCreaft);
        Debug.Log("Success Craft Item : " + _itemToCreaft.name);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlaks = Time.time > lastTimeUsedFalsk + flaskCooldown;

        if(canUseFlaks)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ExcuteItemEffect();
            lastTimeUsedFalsk = Time.time;
        }
    }

    public void UseQuickSlot()
    {

    }

    private void Update()
    {
        
    }
}
