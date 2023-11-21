using UnityEngine;

public abstract class Consumable : Item
{
    /**
     * How many consumables are stacked on one another
     */
    [SerializeField]
    private int count;

    public int Count { get { return count; } set { count = value; } }

    public abstract void Consume();
}
