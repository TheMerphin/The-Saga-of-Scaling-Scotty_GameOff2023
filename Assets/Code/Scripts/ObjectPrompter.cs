using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrompter : MonoBehaviour
{
    public Canvas promptCanvas;

    public bool ShowAlways = false;

    private void Start()
    {
        if (ShowAlways) promptCanvas.enabled = true;
    }

    private void Update()
    {
        if (!ShowAlways) ShowPrompt(false);
    }

    public void ShowPrompt(bool show)
    {
        if (!ShowAlways) promptCanvas.enabled = show;
    }
}
