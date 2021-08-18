using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class colorPicker : MonoBehaviour, IPointerUpHandler, IPointerDownHandler // required interface when using the OnPointerDown method.
{
    public Pencil p;
    public void OnPointerDown(PointerEventData eventData)
    {
        p.ActivateColorPicker();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        p.ActivateColorPicker();
    }
}
