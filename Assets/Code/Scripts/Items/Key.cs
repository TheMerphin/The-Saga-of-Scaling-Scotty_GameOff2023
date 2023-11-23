using UnityEngine;

public class Key : Item
{
    [Header("Key Settings")]
    [SerializeField]
    private Unlocks[] unlocks;
    public Unlocks[] Unlocks { get { return unlocks; } set { unlocks = value; } }
}

public enum Unlocks
{
    None,
    Chests,
    Doors
}
