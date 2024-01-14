using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EdgeFallBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D entityCollider;
    private IActivityToggle activityToggle;

    private AudioManager audioManager;

    private bool isPlayer;

    void Start()
    {
        isPlayer = GetComponent<PlayerController>() != null;

        rb = GetComponent<Rigidbody2D>();
        entityCollider = GetComponent<Collider2D>();
        activityToggle = GetComponent<IActivityToggle>();

        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public void FallOffGround(float fallDepth = 10)
    {
        FallOffGround(Vector2.zero, 0, fallDepth, false);
    }

    public void FallOffGround(Vector2 respawnPosition)
    {
        FallOffGround(respawnPosition, 0);
    }

    public void FallOffGround(Vector2 respawnPosition, float initialDelay, float fallDepth = 8, bool shouldRespawn = true)
    {
        activityToggle.DisableActivity();
        StartCoroutine(FallOffGround_Coroutine(respawnPosition, initialDelay, shouldRespawn, fallDepth));
    }

    private IEnumerator FallOffGround_Coroutine(Vector2 respawnPosition, float initialDelay, bool shouldRespawn, float fallDepth = 8)
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(initialDelay);

        entityCollider.enabled = false;
        var startingPosY = transform.position.y;
        var originalDrag = rb.drag;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (isPlayer)
        {
            GetComponentInChildren<Light2D>().enabled = false;
            FindFirstObjectByType<CinemachineVirtualCamera>().Follow = null;
        }

        rb.gravityScale = 0.7f;
        rb.drag = 0f;
        rb.velocityY = -1f;

        audioManager.Play("PlayerFalling");

        while (startingPosY - fallDepth < transform.position.y)
        {
            if (Mathf.Abs(transform.position.y - (int)transform.position.y) >= 0.875f && spriteRenderer.sortingLayerName.Equals("1_OnGround"))
            {
                spriteRenderer.sortingLayerName = "-1_BelowGround";
                spriteRenderer.sortingOrder = 1;
            }
            yield return new WaitForEndOfFrame();
        }

        spriteRenderer.enabled = false;
        rb.gravityScale = 0f;
        rb.drag = originalDrag;

        // Respawn
        if (!shouldRespawn) yield break;

        yield return new WaitForSeconds(0.5f);
        var isDamageable = GetComponent<IDamageable>();
        if (isDamageable == null || (isDamageable != null && GetComponent<StatManager>().GetIntValue(StatManager.STAT_ID_HEALTH) > 0))
        {
            transform.position = respawnPosition;
            entityCollider.enabled = true;

            if (isPlayer)
            {
                FindFirstObjectByType<CinemachineVirtualCamera>().Follow = transform;
                GetComponentInChildren<Light2D>().enabled = true;
            }

            spriteRenderer.enabled = true;
            spriteRenderer.sortingLayerName = "1_OnGround";
            spriteRenderer.sortingOrder = 0;
            activityToggle.EnableActivity();
        }
    }
}
