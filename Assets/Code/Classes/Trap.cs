using UnityEngine;
using static Toolbox;

public abstract class Trap : MonoBehaviour
{
    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;
    protected AnimationClipOverrides activateClipOverrides;

    protected Collider2D triggerAreaCollider;
    protected AudioManager audioManager;

    protected GameObject _triggerObject;

    [SerializeField]
    private string trapName;
    public string TrapName { get { return trapName; } set { trapName = value; } }

    [SerializeField]
    private bool active = true;
    public bool Active { get { return active; } set { active = value; } }

    [SerializeField]
    private float damage;
    private float _damage;
    public float Damage { get { return _damage; } set { _damage = value; } }

    [SerializeField]
    private float damageDelay = 0;
    public float DamageDelay { get { return damageDelay; } set { damageDelay = value; } }

    [SerializeField]
    private AnimationClip trapAnimation;
    public AnimationClip TrapAnimation { get { return trapAnimation; } set { trapAnimation = value; } }

    [SerializeField]
    private Sound trapSound;
    public Sound TrapSound { get { return trapSound; } set { trapSound = value; } }


    protected static string animatorTriggerTrapId = "TriggerTrap";
    protected virtual void Awake()
    {
        _damage = damage;
        triggerAreaCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioManager = FindFirstObjectByType<AudioManager>();

        triggerAreaCollider.enabled = active;

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        activateClipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(activateClipOverrides);

        activateClipOverrides["Activate"] = trapAnimation;
        animator.Rebind();
        animatorOverrideController.ApplyOverrides(activateClipOverrides);
    }

    protected virtual void Start()
    {
        audioManager.AddSound(trapSound);
    }

    public virtual void TriggerTrap(GameObject triggeringObject, bool isDamageable)
    {
        if (!active) return;
        active = false;

        audioManager.Play(trapSound.name);
        animator.SetTrigger(animatorTriggerTrapId);

        _triggerObject = triggeringObject;
        if (isDamageable) Invoke(nameof(ApplyDamage), damageDelay);

        triggerAreaCollider.enabled = false;
        this.enabled = false;
    }

    protected void ApplyDamage()
    {
        _triggerObject.GetComponent<IDamageable>().TakeDamage(damage);
    }
}
