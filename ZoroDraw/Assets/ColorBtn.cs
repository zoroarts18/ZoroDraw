using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBtn : MonoBehaviour
{
    public Pencil p;
    void Start()
    {
        p = GameObject.FindWithTag("Pen").GetComponent<Pencil>();
        GetComponent<Button>().onClick.AddListener(SetColore);
    }

    public void SetColore()
    {
        p.SetColor(this.gameObject.GetComponent<Button>());
    }
}
