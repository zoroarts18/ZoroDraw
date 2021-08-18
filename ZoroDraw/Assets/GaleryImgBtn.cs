using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaleryImgBtn : MonoBehaviour
{
    public GameObject Image;
    public Pencil p;
    public bool Vert;
    public GameObject HorImg, VertImg;


    void Start()
    {
        p = GameObject.FindWithTag("Pen").GetComponent<Pencil>();
        HorImg = p.ImageVorschauHor;
        VertImg = p.ImageVorschauVert;
        GetComponent<Button>().onClick.AddListener(LookAtPicture);   
    }

    public void LookAtPicture()
    {
        if (Vert)
        {
            VertImg.transform.parent.gameObject.SetActive(true);
            VertImg.GetComponent<VorschauBildLogic>().tex = GetComponent<RawImage>().texture as Texture2D;
            VertImg.GetComponent<VorschauBildLogic>().SetSprite();
        }

        else
        {
            HorImg.transform.parent.gameObject.SetActive(true);
            HorImg.GetComponent<VorschauBildLogic>().tex = GetComponent<RawImage>().texture as Texture2D;
            HorImg.GetComponent<VorschauBildLogic>().SetSprite();
        }
    }
}
