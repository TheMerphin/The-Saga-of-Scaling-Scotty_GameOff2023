using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected Collider2D pickUpCollider;

    protected PlayerController player;

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

    protected virtual void OnAwake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickUpCollider = GetComponent<Collider2D>();

        pickUpCollider.enabled = true;
        spriteRenderer.sprite = icon;
        player = null;

        // Activate item outline
        spriteRenderer.material.SetFloat("Toggle", 1f);
        spriteRenderer.material.SetTexture("_MainTex", icon.texture);
    }

    protected virtual void OnStart()
    {

    }

    /**
     * Called by the player with the respective player reference to pickup the item and initialize
     * everything important, e.g. for weapons changing the attack animations, swapping the current
     * item and dropping it to the ground, etc.
     */
    public virtual void PickUp(Transform playerTransform)
    {
        // Bind gameobject to player, disable spriterenderer
        transform.SetParent(playerTransform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        spriteRenderer.enabled = false;
        player = playerTransform.GetComponent<PlayerController>();
        pickUpCollider.enabled = false;
    }

    public virtual void Drop()
    {
        // Bind gameobject to player, disable spriterenderer
        transform.SetParent(null, true);
        spriteRenderer.enabled = true;
        player = null;
        pickUpCollider.enabled = true;
    }
}
