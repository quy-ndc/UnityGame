using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoostItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMight *= 1 + passiveItemData.Mutiplier / 100f;
    }
}
