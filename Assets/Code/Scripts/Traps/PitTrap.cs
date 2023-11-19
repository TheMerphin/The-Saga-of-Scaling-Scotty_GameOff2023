using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : Trap 
{
    private SpriteRenderer spriteRenderer;

    private Collider2D dynamicCollider;

    [SerializeField]
    private Vector2 playerRespawnPosition;
    public Vector2 PlayerRespawnPosition { get { return playerRespawnPosition; } set { playerRespawnPosition = value; } }

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
            player.GetComponent<PlayerController>().FallOffGround(playerRespawnPosition, 0.075f);
            StartCoroutine(ActivateDynamicCollider());
        }

        base.TriggerTrap(player);
    }

    private IEnumerator ActivateDynamicCollider()
    {
        yield return new WaitForSeconds(0.5f);
        dynamicCollider.enabled = true;
    }
}
