using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;

    [SerializeField] private Image quickSlot1Base;
    [SerializeField] private Image quickSlot1;
    [SerializeField] private Image quickSlot2Base;
    [SerializeField] private Image quickSlot2;

    private SkillManager skills;

    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }

    private void Update()
    {
        UpdateQuickSlotIcon(0, quickSlot1);
        UpdateQuickSlotIcon(0, quickSlot1Base);

        UpdateQuickSlotIcon(1, quickSlot2);
        UpdateQuickSlotIcon(1, quickSlot2Base);

        if (Input.GetKeyDown(KeyCode.Z))
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCooldownOf(quickSlot1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCooldownOf(quickSlot2);

        CheckCooldownOf(dashImage, skills.dash.coolDown);
        CheckCooldownOf(quickSlot1, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot2, Inventory.Instance.usableItemCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void UpdateQuickSlotIcon(int slotIndex, Image quickSlotImage)
    {
        if (Inventory.Instance.usable != null && Inventory.Instance.usable.Count > slotIndex)
        {
            InventoryItem quickSlotItem = Inventory.Instance.usable[slotIndex];
            if (quickSlotItem != null && quickSlotItem.data != null)
            {
                quickSlotImage.sprite = quickSlotItem.data.icon;
                quickSlotImage.enabled = true;
            }
            else
            {
                quickSlotImage.enabled = false;
            }
        }
    }
}
