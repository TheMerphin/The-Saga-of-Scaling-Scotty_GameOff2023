using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLockController : LockController
{
    [Header("Additional Chest Settings")]
    public bool DropsRandomItem = true;
    public GameObject[] RandomItemPool;

    [Space(10)]
    public bool DropsSpecificItem;
    public Item SpecificItem;

    private ParticleSystem openingParticles;

    protected override void Awake()
    {
        if (DropsRandomItem && DropsSpecificItem) Debug.LogError("Chest " + transform.name + " is configured to drop a random and a specific item. This results in a conflict, please select either one of them.");
        if (DropsRandomItem && RandomItemPool.Length == 0) Debug.LogError("Chest " + transform.name + " is configured to drop a random item, but no items are declared in the item pool. Please add at least one to the item pool.");
        if (DropsSpecificItem && SpecificItem == null) Debug.LogError("Chest " + transform.name + " is configured to drop a specific item, but none is declared. Please add the item reference.");

        openingParticles = GetComponentInChildren<ParticleSystem>();

        base.Awake();
    }

    protected override void UnlockSuccessful()
    {
        openingParticles.Play();

        Item itemPrefab;
        if (DropsRandomItem)
        {
            itemPrefab = RandomItemPool[Random.Range(0, RandomItemPool.Length)].GetComponent<Item>();

        }
        else
        {
            itemPrefab = SpecificItem;
        }

        var item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        item.CanBePickedUp = 0;
        item.GetComponent<Animator>().SetTrigger("ChestDrop");

        base.UnlockSuccessful();
    }
}
