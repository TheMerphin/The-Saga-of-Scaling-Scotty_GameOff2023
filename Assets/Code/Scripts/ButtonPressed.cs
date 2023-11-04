using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI textMeshPro;
    public float textClickOffset = 20f;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.IsInteractable()) return;
        var textPos = textMeshPro.rectTransform.localPosition;
        textMeshPro.rectTransform.localPosition = new Vector3(textPos.x, textPos.y - textClickOffset);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.IsInteractable()) return;
        var textPos = textMeshPro.rectTransform.localPosition;
        textMeshPro.rectTransform.localPosition = new Vector3(textPos.x, textPos.y + textClickOffset);
    }
}
