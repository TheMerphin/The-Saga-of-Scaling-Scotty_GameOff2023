using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable
{

    public int healthRegenerated=2;
    public override void Consume()
    {
        base.player.updateHealth(healthRegenerated);
        Destroy(this.gameObject);
        // destroy potion
    }
}
