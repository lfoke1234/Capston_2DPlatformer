using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player Drops")]
    [SerializeField] private float chanceToLooseItem;

    public override void GenerateDrop()
    {
        base.GenerateDrop();

    }
}
