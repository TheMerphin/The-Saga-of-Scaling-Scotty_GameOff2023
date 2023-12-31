using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PitTrap : Trap 
{
    private SpriteRenderer spriteRenderer;

    private Collider2D dynamicCollider;

    [SerializeField]
    private bool oneTimeUse = true;
    public bool OneTimeUse { get { return oneTimeUse; } set { oneTimeUse = value; } }

    [SerializeField]
    private Vector2 playerRespawnPosition;
    public Vector2 PlayerRespawnPosition { get { return playerRespawnPosition; } set { playerRespawnPosition = value; } }

    [SerializeField]
    private float pitDepth = 8f;
    public float PitDepth { get { return pitDepth; } set { pitDepth = value; } }

    [SerializeField]
    private PitTrap[] linkedPitTraps;
    public PitTrap[] LinkedPitTraps { get { return linkedPitTraps; } set { linkedPitTraps = value; } }

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
        dynamicCollider = transform.GetChild(0).GetComponent<Collider2D>();
    }

    public override void TriggerTrap(PlayerController player)
    {
        if (!Active) return;

        spriteRenderer.sortingLayerName = "-1_BelowGround";

        if (player != null)
        {
            Array.ForEach(linkedPitTraps, pitTrap => pitTrap.TriggerTrap(null));
            if ((int)player.ScalingLevelInfo.ScaleLevel < 1 || linkedPitTraps.Length > 0)
            {
                player.GetComponent<PlayerController>().FallOffGround(playerRespawnPosition, 0.075f, pitDepth);
            }
        }

        StartCoroutine(ActivateDynamicCollider());
        if ((int)player.ScalingLevelInfo.ScaleLevel < 1) {
            base.TriggerTrap(player);
        }
        else
        {
            base.TriggerTrap(null);
        }

        if(!oneTimeUse) StartCoroutine(ReactivateTrap());
    }

    private IEnumerator ReactivateTrap()
    {
        yield return new WaitForSeconds(2f);

        spriteRenderer.sortingLayerName = "0_Ground";
        dynamicCollider.enabled = false;

        Active = true;

        animator.Rebind();

        triggerAreaCollider.enabled = true;
        this.enabled = true;
    }

    private IEnumerator ActivateDynamicCollider()
    {
        yield return new WaitForSeconds(0.5f);
        dynamicCollider.enabled = true;
    }
}
