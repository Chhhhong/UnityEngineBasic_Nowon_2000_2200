using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHPPosionS : Item
{
    public override void OnEarn()
    {
        Player.instance.hp += 1;
    }
}
