using System;
using System.Collections;
using UnityEngine;

public class PitTrap : Trap 
{
    private SpriteRenderer spriteRenderer;

    private Collider2D dynamicCollider;

    [SerializeField]
    private bool oneTimeUse = true;
    public bool OneTimeUse { get { return oneTimeUse; } set { oneTimeUse = value; } }

    [SerializeField]
    private Vector2 entityRespawnPosition;
    public Vector2 EntityRespawnPosition { get { return entityRespawnPosition; } set { entityRespawnPosition = value; } }

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

    public override void TriggerTrap(GameObject triggeringObject, bool isDamageable)
    {
        if (!Active) return;

        spriteRenderer.sortingLayerName = "-1_BelowGround";

        Array.ForEach(linkedPitTraps, pitTrap => pitTrap.TriggerTrap(null, false));

        var edgeFallBehaviour = triggeringObject.GetComponent<EdgeFallBehaviour>();
        if (edgeFallBehaviour != null && edgeFallBehaviour.enabled)
        {
            var player = triggeringObject.GetComponent<PlayerController>();
            if (player != null)
            {
                if ((int)player.ScalingLevelInfo.ScaleLevel < 1 || linkedPitTraps.Length > 0)
                {
                    edgeFallBehaviour.FallOffGround(entityRespawnPosition, 0.075f, pitDepth);
                    base.TriggerTrap(edgeFallBehaviour.gameObject, isDamageable);
                }
                else
                {
                    base.TriggerTrap(null, false);
                }
            }
            else
            {
                edgeFallBehaviour.FallOffGround(entityRespawnPosition, 0.075f, pitDepth, false);
                base.TriggerTrap(edgeFallBehaviour.gameObject, edgeFallBehaviour.GetComponent<IDamageable>() != null);
            }
        }
        else
        {
            base.TriggerTrap(null, false);
        }

        StartCoroutine(ActivateDynamicCollider());

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
