using System.Collections;
using UnityEngine;
using static Toolbox;

public class FallingStone : Trap
{
    private Animator shadowAnimator;
    private AnimatorOverrideController shadowAnimatorOverrideController;
    private AnimationClipOverrides shadowActivateClipOverrides;

    private Collider2D dynamicCollider;

    [SerializeField]
    private AnimationClip shadowAnimation;
    public AnimationClip ShadowAnimation { get { return shadowAnimation; } set { shadowAnimation = value; } }

    [SerializeField]
    private Sound stoneHitSound;
    public Sound StoneHitSound { get { return StoneHitSound; } set { StoneHitSound = value; } }

    protected override void Awake()
    {
        base.Awake();

        shadowAnimator = transform.GetChild(0).GetComponent<Animator>();
        dynamicCollider = transform.GetChild(1).GetComponent<Collider2D>();

        shadowAnimatorOverrideController = new AnimatorOverrideController(shadowAnimator.runtimeAnimatorController);
        shadowAnimator.runtimeAnimatorController = shadowAnimatorOverrideController;

        shadowActivateClipOverrides = new AnimationClipOverrides(shadowAnimatorOverrideController.overridesCount);
        shadowAnimatorOverrideController.GetOverrides(shadowActivateClipOverrides);

        shadowActivateClipOverrides["Activate"] = shadowAnimation;
        shadowAnimator.Rebind();
        shadowAnimatorOverrideController.ApplyOverrides(shadowActivateClipOverrides);
    }

    protected override void Start()
    {
        base.Start();

        audioManager.AddSound(stoneHitSound);
    }

    public override void TriggerTrap(PlayerController player)
    {
        if (!Active) return;

        shadowAnimator.SetTrigger(animatorTriggerTrapId);

        base.TriggerTrap(player);
    }

    public void PlayHitGroundSound()
    {
        audioManager.Play(stoneHitSound.name);
        StartCoroutine(ActivateDynamicCollider());
    }

    private IEnumerator ActivateDynamicCollider()
    {
        yield return new WaitForSeconds(0.4f);
        dynamicCollider.enabled = true;
    }
}
