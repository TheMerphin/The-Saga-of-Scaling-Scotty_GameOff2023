using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrompter : MonoBehaviour
{
    public Canvas promptCanvas;

    public bool ShowAlways = false;

    public bool DisablePrompt = false;

    private void Start()
    {
        if (ShowAlways && !DisablePrompt)
        {
            promptCanvas.enabled = true;
        }
        else
        {
            promptCanvas.enabled = false;
        }
    }

    private void Update()
    {
        if (!ShowAlways) ShowPrompt(false);
    }

    public void ShowPrompt(bool show)
    {
        if (!ShowAlways && !DisablePrompt) promptCanvas.enabled = show;
    }
}
