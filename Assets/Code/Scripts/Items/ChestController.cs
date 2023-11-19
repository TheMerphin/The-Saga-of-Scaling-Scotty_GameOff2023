using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ChestController : MonoBehaviour
{
    public bool DropsRandomItem = true;
    public GameObject[] RandomItemPool;

    [Space(10)]
    public bool DropsSpecificItem;
    public Item SpecificItem;

    [Space(10)]
    public bool KeyRequired;
    public Item KeyReference;

    private Animator animator;
    private AudioManager audioManager;
    private ParticleSystem openingParticles;

    private GameObject noKeyPrompt;
    private GameObject keyPrompt;

    private void Awake()
    {
        if (DropsRandomItem && DropsSpecificItem) Debug.LogError("Chest " + transform.name + " is configured to drop a random and a specific item. This results in a conflict, please select either one of them.");
        if (DropsRandomItem && RandomItemPool.Length == 0) Debug.LogError("Chest " + transform.name + " is configured to drop a random item, but no items are declared in the item pool. Please add at least one to the item pool.");
        if (DropsSpecificItem && SpecificItem == null) Debug.LogError("Chest " + transform.name + " is configured to drop a specific item, but none is declared. Please add the item reference.");
        if (KeyRequired && KeyReference == null) Debug.LogError("Chest " + transform.name + " is configured to require a key, but none is declared. Please add the key reference.");

        animator = GetComponent<Animator>();
        audioManager = FindFirstObjectByType<AudioManager>();
        openingParticles = GetComponentInChildren<ParticleSystem>();

        keyPrompt = transform.Find("#Chest_Prompt/Prompt_KeyRequired").gameObject;
        noKeyPrompt = transform.Find("#Chest_Prompt/Prompt_Unrestricted").gameObject;

        if (KeyRequired)
        {
            keyPrompt.SetActive(true);
        }
        else
        {
            noKeyPrompt.SetActive(true);
        }
    }

    /**
     * Opens the chest if all conditions are fulfilled.
     * Returns whether opening the chest was successful or not.
     */
    public bool OpenChest(Item keyReference = null)
    {
        if (!KeyRequired || (KeyRequired && keyReference == KeyReference))
        {
            animator.SetTrigger("opening");
            audioManager.Play("ChestOpening");
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
            DisableChest();
            return true;
        }

        audioManager.Play("Locked");
        return false;
    }

    private void DisableChest()
    {
        GetComponent<Collider2D>().enabled = false;
        var objectPrompter = GetComponent<ObjectPrompter>();
        objectPrompter.DisablePrompt = true;
        objectPrompter.enabled = false;
        this.enabled = false;
    }

    public void UpdatePrompt(bool hasKey)
    {
        if (!KeyRequired) return;
        noKeyPrompt.SetActive(hasKey);
        keyPrompt.SetActive(!hasKey);
    }
}
