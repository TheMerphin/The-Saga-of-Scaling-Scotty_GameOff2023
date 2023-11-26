using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Item KeyReference;

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
    }

    public bool OpenDoor(Item keyReference = null)
    {
        if (KeyReference == keyReference)
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
        GetComponent<Collider2D>().enabled = false;
        var objectPrompter = GetComponent<ObjectPrompter>();
        objectPrompter.DisablePrompt = true;
        objectPrompter.enabled = false;
        GetComponent<DoorController>().enabled = false;
    }

    public void UpdatePrompt(bool hasKey)
    {
        hasKeyPrompt.SetActive(hasKey);
        hasNoKeyPrompt.SetActive(!hasKey);
    }
}
