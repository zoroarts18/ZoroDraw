using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VorschauBildLogic : MonoBehaviour
{
    public Texture2D tex;

    void Start()
    {
        
        GetComponentInChildren<Button>().onClick.AddListener(closeImage);
    }

    public void SetSprite()
    {
        GetComponent<RawImage>().texture = tex;
    }

    public void closeImage()
    {
        DeactivateImage();
    }

    public void DeactivateImage()
    {
        //GetComponent<Image>().sprite = null;
        transform.parent.gameObject.SetActive(false);
    }

}
