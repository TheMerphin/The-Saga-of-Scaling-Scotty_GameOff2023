using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Toolbox;

public class PlayerController : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Animator animator;

    private float horizontalMovement, verticalMovement;
    private bool isFacingTL, isFacingBR, isFacingBL, isFacingTR;

    private GameMenuController gameMenuController;
    private AudioManager audioManager;
    private GameManager gameManager;

    private float interactCooldown;

    private PlayerScalingInfo scalingLevelInfo;
    public PlayerScalingInfo ScalingLevelInfo { get { return scalingLevelInfo; } set { scalingLevelInfo = value; } }

    private StatManager statManager;

    private ItemManager itemManager;

    private bool scaleCooldown = false;

    private bool disableInputs = false;

    private bool isDead = false;

    private StepSoundScript stepSoundController;

    private ParticleSystem movementParticles;
    private ParticleSystem damageParticles;

    public static string STAT_ID_HEALTH = "Health";
    public static string STAT_ID_MOVEMENT_SPEED_FACTOR = "MovementSpeedFactor";

    private void Awake()
    {
        isFacingBL = isFacingBR = isFacingTL = isFacingTR = false;
        interactCooldown = 0f;
        scalingLevelInfo = GetScaleStructByScaleLevel(ScaleLevel.Normal);
        transform.localScale = Vector3.one;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        itemManager = GetComponent<ItemManager>();

        gameMenuController = FindFirstObjectByType<GameMenuController>();
        audioManager = FindFirstObjectByType<AudioManager>();
        gameManager = FindFirstObjectByType<GameManager>();

        gameManager = FindFirstObjectByType<GameManager>();

        stepSoundController = GetComponent<StepSoundScript>();

        damageParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        movementParticles = transform.GetChild(1).GetComponent<ParticleSystem>();

        statManager = GetComponent<StatManager>();
        statManager.AddStats(
            new EntityStatInt(STAT_ID_HEALTH, RangeInt.of(0, 10), 10),
            new EntityStatFloat(STAT_ID_MOVEMENT_SPEED_FACTOR, RangeFloat.of(0, 2), 1)
            );

        gameMenuController.SetMaxHealth(statManager.GetIntValueRange(STAT_ID_HEALTH).Max);
    }

    void Update()
    {
        ResetValuesBeforeFrame();
        HandleInput();
    }

    void LateUpdate()
    {
        if (disableInputs) return;

        HandleInteraction();
        HandleDungeonExits();
    }

    void FixedUpdate()
    {
        if (horizontalMovement > 0.01f || horizontalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(horizontalMovement * statManager.GetFloatValue(STAT_ID_MOVEMENT_SPEED_FACTOR) * 2f, 0f), ForceMode2D.Impulse);
        }

        if (verticalMovement > 0.01f || verticalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(0f, verticalMovement * statManager.GetFloatValue(STAT_ID_MOVEMENT_SPEED_FACTOR) * 1.75f), ForceMode2D.Impulse);
        }
    }

    public void HandleInteraction()
    {
        var interactable = Physics2D.CircleCast(transform.position, 1.5f, Vector2.zero, 0f, LayerMask.GetMask("Interactable"));
        if (interactable.collider != null)
        {
            var item = interactable.collider.GetComponent<Item>();
            var door = interactable.collider.GetComponent<DoorController>();
            var chest = interactable.collider.GetComponent<ChestController>();
            var prompter = interactable.collider.GetComponent<ObjectPrompter>();

            if (Input.GetKeyDown(KeyCode.F) && interactCooldown <= 0f && item != null && item.CanBePickedUp == 1)
            {
                interactCooldown = 1f;
                itemManager.PickUpItem(item);
                return;
            }

            if (chest != null)
            {
                if (chest.KeyRequired && itemManager.CarriesKey() && itemManager.GetKey().Unlocks.Contains(chest.LockType))
                {
                    chest.UpdatePrompt(true);
                }
                else
                {
                    chest.UpdatePrompt(false);
                }

                if (Input.GetKeyDown(KeyCode.F) && chest.KeyRequired && itemManager.CarriesKey() && chest.OpenChest(itemManager.GetKey()))
                {
                    itemManager.ClearSlot(4);
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    chest.OpenChest();
                }
            }

            if (door != null)
            {
                if (!door.KeyRequired || (door.KeyRequired && itemManager.CarriesKey() && itemManager.GetKey().Unlocks.Contains(door.LockType)))
                {
                    door.UpdatePrompt(true);
                }
                else
                {
                    door.UpdatePrompt(false);
                }

                if (Input.GetKeyDown(KeyCode.F) && door.KeyRequired && itemManager.CarriesKey() && door.OpenDoor(itemManager.GetKey()))
                {
                    itemManager.ClearSlot(4);
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    door.OpenDoor();
                }
            }

            if (prompter != null)
            {
                prompter.ShowPrompt(true);
            }
        }
    }

    public void HandleDungeonExits()
    {
        var exitCast = Physics2D.CircleCast(transform.position, 0.01f, Vector2.zero, 0f, LayerMask.GetMask("Exit"));
        if (exitCast.collider != null)
        {
            var exitController = exitCast.collider.GetComponent<ExitController>();
            if (exitController != null) StartCoroutine(ExitLevelViaStair(exitController));
        };
    }

    private void HandleInput()
    {
        if (disableInputs) return;

        HandleMovement();
        HandleActions();
    }

    private void HandleMovement()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Evaluate player direction
        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDirection = (mousePos - playerPos).normalized;

        switch (GetDiagonalDirection(mouseDirection))
        {
            case DiagonalDirection.UpRight:
                isFacingTR = true;
                break;
            case DiagonalDirection.UpLeft:
                isFacingTL = true;
                break;
            case DiagonalDirection.DownRight:
                isFacingBR = true;
                break;
            case DiagonalDirection.DownLeft:
                isFacingBL = true;
                break;
        }

        animator.SetBool("isFacingTR", isFacingTR);
        animator.SetBool("isFacingTL", isFacingTL);
        animator.SetBool("isFacingBR", isFacingBR);
        animator.SetBool("isFacingBL", isFacingBL);

        if (horizontalMovement != 0f | verticalMovement != 0f)
        {
            animator.SetBool("isMoving", true);
            movementParticles.Play();
        }
    }

    private void HandleActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var selectedItem = itemManager.GetSelectedItem();
            if (selectedItem is Weapon)
            {
                var currentClips = animator.GetCurrentAnimatorClipInfo(0);
                var firstClip = currentClips.Length > 0 ? currentClips[0].clip : null;

                if (firstClip == null || (firstClip != null && !firstClip.name.Contains("Attack")))
                {
                    animator.SetTrigger("Attack");
                    (selectedItem as Weapon).Attack();
                }
            }
            else if (selectedItem is Consumable)
            {
                (selectedItem as Consumable).Consume();
                audioManager.Play("Chug"); // TODO should be called inside the specific consumable
                itemManager.ClearSlot(3);
            }
        }

        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift)) && scaleCooldown)
        {
            audioManager.Play("OnCooldown");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !scaleCooldown && (int)scalingLevelInfo.ScaleLevel < 1)
        {
            ScalePlayerUp();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !scaleCooldown && (int)scalingLevelInfo.ScaleLevel > -1)
        {
            ScalePlayerDown();
        }

        var mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll < 0f)
        {
            itemManager.AdvanceSelectedSlot();
        }
        else if (mouseScroll > 0f)
        {
            itemManager.RetreatSelectedSlot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) itemManager.SetSelectedSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) itemManager.SetSelectedSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) itemManager.SetSelectedSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) itemManager.SetSelectedSlot(3);
    }
    private void ResetValuesBeforeFrame()
    {
        isFacingBL = isFacingBR = isFacingTL = isFacingTR = false;

        animator.SetBool("isFacingTR", false);
        animator.SetBool("isFacingTL", false);
        animator.SetBool("isFacingBR", false);
        animator.SetBool("isFacingBL", false);
        animator.SetBool("isMoving", false);

        if (interactCooldown > 0f) interactCooldown -= 0.01f;

        horizontalMovement = verticalMovement = 0f;

        movementParticles.Pause();
    }

    private void ScalePlayerUp()
    {
        ScaleLevel targetScaleLevel;
        if (scalingLevelInfo.ScaleLevel.Equals(ScaleLevel.Normal))
        {
            targetScaleLevel = ScaleLevel.Big;
        }
        else  // ScaleLevel.Small
        {
            targetScaleLevel = ScaleLevel.Normal;
        }

        StartCoroutine(Scale(targetScaleLevel));
    }

    private void ScalePlayerDown()
    {
        ScaleLevel targetScaleLevel;
        if (scalingLevelInfo.ScaleLevel.Equals(ScaleLevel.Normal))
        {
            targetScaleLevel = ScaleLevel.Small;
        }
        else // ScaleLevel.Big
        {
            targetScaleLevel = ScaleLevel.Normal;
        }

        StartCoroutine(Scale(targetScaleLevel));
    }

    public DiagonalDirection GetOrientation()
    {
        if (isFacingTL) return DiagonalDirection.UpLeft;
        if (isFacingTR) return DiagonalDirection.UpRight;
        if (isFacingBL) return DiagonalDirection.DownLeft;
        return DiagonalDirection.DownRight;
    }

    private IEnumerator Scale(ScaleLevel targetScaleLevel)
    {
        StartCoroutine(ScaleCooldown());

        var targetScalingInfo = GetScaleStructByScaleLevel(targetScaleLevel);
        var currentTransformScale = scalingLevelInfo.TransformScale;
        var currentStepSoundPitch = scalingLevelInfo.StepSoundPitchModifier;
        var currentMovementSpeed = scalingLevelInfo.MovementSpeedModifier;

        var totalDuration = 1f;
        var ticks = 20f;

        if ((int)scalingLevelInfo.ScaleLevel < (int)targetScalingInfo.ScaleLevel)
        {
            audioManager.Play("Inflate");
        }
        else
        {
            audioManager.Play("Deflate");
        }

        for (int i = 0; i <= ticks; i++)
        {
            var transformLerp = Mathf.Lerp(currentTransformScale, targetScalingInfo.TransformScale, i / ticks);
            transform.localScale = new Vector2(transformLerp, transformLerp);

            var movementSpeedLerp = Mathf.Lerp(currentMovementSpeed, targetScalingInfo.MovementSpeedModifier, i / ticks);
            animator.SetFloat("movementSpeedMultiplier", movementSpeedLerp);
            statManager.SetStatValue(STAT_ID_MOVEMENT_SPEED_FACTOR, movementSpeedLerp);

            var stepSoundPitchLerp = Mathf.Lerp(currentStepSoundPitch, targetScalingInfo.StepSoundPitchModifier, i / ticks);
            stepSoundController.PitchFactor = stepSoundPitchLerp;

            if (i == ticks / 2f) scalingLevelInfo = targetScalingInfo;
            yield return new WaitForSeconds(totalDuration / ticks);
        }

        // Update items
        itemManager.OnPlayerScaleChange(scalingLevelInfo);
    }

    private IEnumerator ScaleCooldown()
    {
        scaleCooldown = true;
        yield return new WaitForSeconds(1.75f);
        scaleCooldown = false;
    }

    //TODO add to general script which enables entities to fall off ground (and possibly into lava/water as well)
    public void FallOffGround(Vector2 respawnPosition)
    {
        FallOffGround(respawnPosition, 0);
    }

    //TODO add to general script which enables entities to fall off ground (and possibly into lava/water as well)
    public void FallOffGround(Vector2 respawnPosition, float initialDelay, float fallDepth = 8)
    {
        disableInputs = true;
        StartCoroutine(FallOffGround_Coroutine(respawnPosition, initialDelay, fallDepth));
    }

    //TODO add to general script which enables entities to fall off ground (and possibly into lava/water as well)
    private IEnumerator FallOffGround_Coroutine(Vector2 respawnPosition, float initialDelay, float fallDepth = 8)
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(initialDelay);

        playerCollider.enabled = false;
        var startingPosY = transform.position.y;
        var originalDrag = rb.drag;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var cinemachineCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        var light = GetComponentInChildren<Light2D>();
        light.enabled = false;
        cinemachineCamera.Follow = null;

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
        yield return new WaitForSeconds(0.5f);
        if (statManager.GetIntValue(STAT_ID_HEALTH) > 0)
        {
            cinemachineCamera.Follow = transform;
            transform.position = respawnPosition;
            playerCollider.enabled = true;
            light.enabled = true;

            spriteRenderer.enabled = true;
            spriteRenderer.sortingLayerName = "1_OnGround";
            spriteRenderer.sortingOrder = 0;
            disableInputs = false;
        }
    }

    private IEnumerator ExitLevelViaStair(ExitController stair)
    {
        disableInputs = true;
        rb.velocity = Vector2.zero;
        playerCollider.enabled = false;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        var startingPosX = transform.position.x;
        var walkingDirection = new Vector2(((int) stair.Direction) * 1, 0.5f);

        if (stair.Direction == StairDirection.Left)
        {
            animator.SetBool("isExitingLeft", true);
        }
        else
        {
            animator.SetBool("isExitingRight", true);
        }

        while (Mathf.Abs(transform.position.x - startingPosX) < 2.3f)
        {
            transform.Translate(walkingDirection * Time.deltaTime * 2.5f);

            if (Mathf.Abs(transform.position.x - startingPosX) >= 1f && spriteRenderer.sortingOrder == 0) spriteRenderer.sortingOrder = 1;
            if (Mathf.Abs(transform.position.x - startingPosX) >= 1f) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 2f - Mathf.Abs(transform.position.x - startingPosX));

            yield return new WaitForEndOfFrame();
        }

        gameManager.ProgressToNextLevel();

        yield return new WaitForSeconds(1);

        animator.SetBool("isExitingLeft", false);
        animator.SetBool("isExitingRight", false);

        transform.position = Vector2.zero;
        disableInputs = false;
        playerCollider.enabled = true;
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.color = Color.white;
    }

    public void TakeDamage(float damage)
    {
        UpdateHealth((int) -damage);
    }

    /*
     * Updates the player health by adding a value. Can be negative to remove health.
    */
    public void UpdateHealth(int additive)
    {
        if (isDead) return;

        statManager.UpdateStatValue(STAT_ID_HEALTH, additive);

        // Damaged
        if (additive <= 0f)
        {
            audioManager.Play("PlayerHurt");
            damageParticles.Play();

            if (statManager.GetIntValue(STAT_ID_HEALTH) <= 0)
            {
                disableInputs = true;
                isDead = true;
                animator.SetBool("isDead", true);
                audioManager.Play("PlayerDeath");
                gameManager.GameOver();
            }
        }

        gameMenuController.SetHealth(statManager.GetIntValue(STAT_ID_HEALTH));
    }
}
