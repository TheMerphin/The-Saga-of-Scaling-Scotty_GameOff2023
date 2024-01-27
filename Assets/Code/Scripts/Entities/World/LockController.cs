using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class LockController : MonoBehaviour
{
    protected Animator animator;
    protected AudioManager audioManager;

    protected GameObject hasKeyPrompt;
    protected GameObject hasNoKeyPrompt;

    [Header("Lock Settings")]

    [SerializeField]
    protected LockType lockType;
    public LockType LockType { get { return lockType; } set { lockType = value; } }

    [SerializeField]
    protected Sprite keyIcon;
    public Sprite KeyIcon { get { return keyIcon; } set { keyIcon = value; } }

    [SerializeField]
    private Sound unlockSound;
    public Sound UnlockSound { get { return unlockSound; } set { unlockSound = value; } }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    protected virtual void Start()
    {
        audioManager.AddSound(unlockSound);
        hasKeyPrompt = transform.Find("#Lock_Prompt/Prompt_HasKey").gameObject;
        hasNoKeyPrompt = transform.Find("#Lock_Prompt/Prompt_HasNoKey").gameObject;

        if (lockType != LockType.None)
        {
            hasNoKeyPrompt.transform.Find("Open/KeyIcon").GetComponent<Image>().sprite = keyIcon;

            hasNoKeyPrompt.SetActive(true);
            hasKeyPrompt.SetActive(false);
        }
        else
        {
            hasNoKeyPrompt.SetActive(false);
            hasKeyPrompt.SetActive(true);
        }
    }

    /**
    * Unlocks the lock if all conditions are fulfilled.
    * Returns whether opening the chest was successful or not.
    * Consuming the key has to be handled by the calling entity.
    */
    public virtual bool Unlock(Key keyReference = null)
    {
        if (lockType == LockType.None || (keyReference != null && keyReference.Unlocks.Contains(lockType)))
        {
            UnlockSuccessful();
            return true;
        }

        audioManager.Play("Locked");
        return false;
    }

    public void UpdateKeyPrompt(bool hasKey)
    {
        if (lockType == LockType.None) return;
        hasKeyPrompt.SetActive(hasKey);
        hasNoKeyPrompt.SetActive(!hasKey);
    }

    public bool KeyRequired()
    {
        return lockType != LockType.None;
    }

    protected virtual void UnlockSuccessful()
    {
        animator.SetTrigger("opening");
        audioManager.Play(unlockSound.name);

        DisableLock();
    }

    protected virtual void DisableLock()
    {
        GetComponent<Collider2D>().enabled = false;

        var objectPrompter = GetComponent<ObjectPrompter>();
        objectPrompter.DisablePrompt = true;
        objectPrompter.enabled = false;

        GetComponent<LockController>().enabled = false;
    }
}
