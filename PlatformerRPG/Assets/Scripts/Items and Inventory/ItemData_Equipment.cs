using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Shose,
    Flask,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major Stat")]
    public int strength; 
    public int agility;
    public int intelligence;
    public int vitality;
    public int recoveryStaminaSpeed;

    [Header("Offensive Stats")]
    public int damage;
    public int trueDamage;
    public int critChacne;
    public int critPower;

    [Header("Defencive stats")]
    public int health;
    public int stamina;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Creaft requrements")]
    public List<InventoryItem> craftingMaterial;

    private int descriptionLength;

    public void ExcuteItemEffect()
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }

    public void AddModifire()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifiers(strength);
        playerStats.agility.AddModifiers(agility);
        playerStats.intelligence.AddModifiers(intelligence);
        playerStats.vitality.AddModifiers(vitality);
        playerStats.recoveryStaminaSpeed.AddModifiers(recoveryStaminaSpeed);

        playerStats.damage.AddModifiers(damage);
        playerStats.trueDamage.AddModifiers(trueDamage);
        playerStats.critChacne.AddModifiers(critChacne);
        playerStats.critPower.AddModifiers(critPower);

        playerStats.maxHealth.AddModifiers(health);
        playerStats.maxStamina.AddModifiers(stamina);
        playerStats.armor.AddModifiers(armor);
        playerStats.evasion.AddModifiers(evasion);
        playerStats.magicResistance.AddModifiers(magicResistance);

        playerStats.posionDamage.AddModifiers(fireDamage);
        playerStats.iceDamage.AddModifiers(iceDamage);
        playerStats.lightingDamage.AddModifiers(lightingDamage);
    }

    public void RemoveModifire() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifiers(strength);
        playerStats.agility.RemoveModifiers(agility);
        playerStats.intelligence.RemoveModifiers(intelligence);
        playerStats.vitality.RemoveModifiers(vitality);
        playerStats.recoveryStaminaSpeed.RemoveModifiers(recoveryStaminaSpeed);

        playerStats.damage.RemoveModifiers(damage);
        playerStats.trueDamage.RemoveModifiers(trueDamage);
        playerStats.critChacne.RemoveModifiers(critChacne);
        playerStats.critPower.RemoveModifiers(critPower);

        playerStats.maxHealth.RemoveModifiers(health);
        playerStats.maxStamina.RemoveModifiers(stamina);
        playerStats.armor.RemoveModifiers(armor);
        playerStats.evasion.RemoveModifiers(evasion);
        playerStats.magicResistance.RemoveModifiers(magicResistance);

        playerStats.posionDamage.RemoveModifiers(fireDamage);
        playerStats.iceDamage.RemoveModifiers(iceDamage);
        playerStats.lightingDamage.RemoveModifiers(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(damage, "Damage");
        AddItemDescription(trueDamage, "True Damage");
        AddItemDescription(health, "Health");
        AddItemDescription(recoveryStaminaSpeed, "Stamina Recover Speed");
        AddItemDescription(armor, "Armor");

        if(descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if(sb.Length > 0)
                sb.AppendLine();

            if(_value > 0)
                sb.Append("+ " + _value + " " + _name);

            descriptionLength++;
        }
    }
}
