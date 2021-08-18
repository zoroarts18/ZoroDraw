using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileContent : MonoBehaviour
{
    public string FileName;
    public bool Vert = true;
    public List<GameObject> Lines;
    public List<GameObject> Erasers;

    public GameObject LayerBtn;
    public Transform LayerBtnHolder;
    public Button currentLayerBtn;
    public List<Button> LayerBtns;
    public int NoumberOfLayers;

    public Color BGColor;
    public int FileNoumber;
    public Pencil p;
    public Button DeleteBtn;

    public void Start()
    {
        DeleteBtn.onClick.AddListener(DeleteFile);
        p = GameObject.FindWithTag("Pen").GetComponent<Pencil>();
        GetComponentInChildren<Text>().text = FileName.ToString();
        GetComponent<Button>().onClick.AddListener(SelectThisFile);
    }

    public void NewLayer()
    {
        GameObject newLayerBtn = Instantiate(LayerBtn) as GameObject;
        currentLayerBtn = newLayerBtn.GetComponent<Button>();
        currentLayerBtn.transform.parent = LayerBtnHolder;
    }

    public void DeleteLayer()
    {

    }

    public void DeleteFile()
    {
        p.DeletePage(GetComponent<Button>());
        
    }

    public void SelectThisFile()
    {
        if(p.currentFileBtn) p.selectOtherFile(this.gameObject);
    }
}
