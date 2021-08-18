using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayerBtn : MonoBehaviour
{
    public Pencil p;
    public int Layer;
    public int AssigneableSortingLayer;
    public List<GameObject> LinesOnLayer;

    public FileContent File;
    public Button HideBtn;
    public Button DeleteBtn;

    private bool isHidden = false;
    public Text LayerText;

    void Start()
    {
        
        GetComponent<Button>().onClick.AddListener(SetThisAsNewLayer);
        HideBtn.onClick.AddListener(Hide);
    }

    public void SetThisAsNewLayer()
    {
        
        /*
        p.currentLayerBtn = this.gameObject;
        p.CurrentLayer = Layer;
        */
    }

    public void AktualisierLines()
    {
        AssigneableSortingLayer = 0;
        for (int i = 0; i < LinesOnLayer.Count; i++)
        {
            AssigneableSortingLayer ++;
            //LinesOnLayer[i].GetComponent<TrailRenderer>().
        }
    }

    public void Hide()
    {
        if(isHidden)
        {
            foreach (var item in LinesOnLayer)
            {
                item.SetActive(true);
            }
            isHidden = false;
        }


        else
        {
            foreach (var item in LinesOnLayer)
            {
                item.SetActive(false);
            }
            isHidden = true;
        }
        
    }
}
