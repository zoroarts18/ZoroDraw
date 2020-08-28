using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public Pencil p;
    private void OnMouseDown()
    {
        p.closeColorPanel();
        p.closeSizePanel();
        p.closeSwitchPanel();
    }
}
