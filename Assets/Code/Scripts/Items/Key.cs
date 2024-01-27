using UnityEngine;

public class Key : Item
{
    [Header("Key Settings")]
    [SerializeField]
    private LockType[] unlocks;
    public LockType[] Unlocks { get { return unlocks; } set { unlocks = value; } }
}

public enum LockType
{
    None,
    Chest,
    Door
}
