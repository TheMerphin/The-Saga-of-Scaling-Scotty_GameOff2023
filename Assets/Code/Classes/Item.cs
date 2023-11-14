using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public abstract class Item : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected Collider2D pickUpCollider;

    protected PlayerController player;

    protected Animator animator;

    protected AudioManager audioManager;

    protected ObjectPrompter objectPrompter;

    [Header("Item Settings")]
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } set { icon = value; } }

    [SerializeField]
    private string itemName;
    public string ItemName { get { return itemName; } set { itemName = value; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } set { description = value; } }

    [SerializeField]
    private int tierClass;
    public int TierClass { get { return tierClass; } set { tierClass = value; } }

    [SerializeField]
    private bool canBePickedUp = true;
    public int CanBePickedUp { get { return canBePickedUp ? 1 : 0; } set { canBePickedUp = value == 1 ? true : false; objectPrompter.DisablePrompt = !canBePickedUp; } }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickUpCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        objectPrompter = GetComponent<ObjectPrompter>();

        pickUpCollider.enabled = true;
        spriteRenderer.sprite = icon;
        player = null;

        // Activate item outline
        spriteRenderer.material.SetFloat("Toggle", 0f);
        spriteRenderer.material.SetTexture("_MainTex", icon.texture);
    }

    protected virtual void Start()
    {
        Array.ForEach(gameObject.GetComponentsInChildren<TextMeshProUGUI>(), text => {
            if (text.name.Equals("#AUTOFILL_Name")) { text.text = ItemName; }
            if (text.name.Equals("#AUTOFILL_Description")) { text.text = Description; }
        });

        audioManager = FindFirstObjectByType<AudioManager>();
    }

    /**
     * Called by the player with the respective player reference to pickup the item and initialize
     * everything important, e.g. for weapons changing the attack animations, swapping the current
     * item and dropping it to the ground, etc.
     */
    public virtual void PickUp(Transform playerTransform)
    {
        StartCoroutine(GetPickedUp(playerTransform));
    }

    public virtual void Drop()
    {
        // Bind gameobject to player, disable spriterenderer
        transform.SetParent(null, true);
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        transform.localRotation = Quaternion.identity;
        spriteRenderer.enabled = true;
        player = null;
        pickUpCollider.enabled = true;
        animator.SetBool("IsPickedUp", false);
    }

    private IEnumerator GetPickedUp(Transform playerTransform)
    {
        var steps = 30f;
        animator.SetBool("IsPickedUp", true);

        player = playerTransform.GetComponent<PlayerController>();
        pickUpCollider.enabled = false;

        for (int i = 0; i < steps - 15f; i++)
        {
            var vectorStep = Vector2.Lerp(transform.position, playerTransform.position, i/steps);
            transform.localPosition = vectorStep;
            transform.localScale = new Vector2(1 - (i / steps), 1 - (i / steps));
            yield return new WaitForSeconds(0.006f);
        }

        // Bind gameobject to player, disable spriterenderer
        transform.SetParent(playerTransform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        spriteRenderer.enabled = false;
    }

    protected void PlayPickUpSound()
    {
        audioManager.Play("ItemPickUp");
    }

    protected void PlayDropSound()
    {
        audioManager.Play("ItemDrop");
    }
}
