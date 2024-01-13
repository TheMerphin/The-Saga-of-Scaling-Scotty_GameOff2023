public class HealthPotion : Consumable
{

    public int healthRegenerated=2;
    public override void Consume()
    {
        base.player.UpdateHealth(healthRegenerated);
        Destroy(this.gameObject);
    }
}
