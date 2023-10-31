using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Item effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect()
    {
    }
}
