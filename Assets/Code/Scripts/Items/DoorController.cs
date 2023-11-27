using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool KeyRequired = true;
    public Unlocks LockType;

    private Animator animator;
    private AudioManager audioManager;

    private GameObject hasKeyPrompt;
    private GameObject hasNoKeyPrompt;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void Start()
    {
        hasKeyPrompt = transform.Find("#Door_Prompt/Prompt_HasKey").gameObject;
        hasNoKeyPrompt = transform.Find("#Door_Prompt/Prompt_HasNoKey").gameObject;

        if (KeyRequired)
        {
            hasNoKeyPrompt.SetActive(true);
            hasKeyPrompt.SetActive(false);
        }
        else
        {
            hasNoKeyPrompt.SetActive(false);
            hasKeyPrompt.SetActive(true);
        }
    }

    public bool OpenDoor(Key keyReference = null)
    {
        if (KeyRequired && keyReference != null && keyReference.Unlocks.Contains(LockType))
        {
            animator.SetTrigger("opening");
            audioManager.Play("DoorOpening");

            DisableDoor();
            return true;
        }

        if (!KeyRequired)
        {
            animator.SetTrigger("opening");
            audioManager.Play("DoorOpening");

            DisableDoor();
            return true;
        }

        audioManager.Play("Locked");
        return false;
    }

    private void DisableDoor()
    {
        transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
        var objectPrompter = GetComponent<ObjectPrompter>();
        objectPrompter.DisablePrompt = true;
        objectPrompter.enabled = false;
        GetComponent<DoorController>().enabled = false;
    }

    public void UpdatePrompt(bool hasKey)
    {
        if (!KeyRequired) return;
        hasKeyPrompt.SetActive(hasKey);
        hasNoKeyPrompt.SetActive(!hasKey);
    }
}
