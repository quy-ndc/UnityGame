using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMoveSpeed *= 1 + passiveItemData.Mutiplier / 100f;
    }
}
