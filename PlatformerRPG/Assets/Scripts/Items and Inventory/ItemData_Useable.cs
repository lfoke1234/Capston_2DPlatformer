using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Usable")]
public class ItemData_Useable : ItemData
{
    public int usableItemID;

    public float itemCooldown;
    public ItemEffect[] itemEffects;

    public void ExcuteItemEffect()
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }

    public override string GetDescription()
    {
        sb.Clear();
        sb.Append(itemDescription);

        return sb.ToString();
    }
}