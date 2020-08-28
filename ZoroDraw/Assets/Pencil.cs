using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pencil : MonoBehaviour
{
    public Button PenButton;
    public Button EraserButton;
    public Button BrushButton;
    public Button colorButton;
    public Button DeleteButton;
    public Button sizeButton;
    public Button FillButton;

    public Button switchButton;

    public Button blackButton;
    public Button redButton;
    public Button greenButton;
    public Button whiteButton;
    public Button blueButton;

    public GameObject Pen;
    public GameObject Eraser;
    public GameObject Brush;

    public GameObject SwitchButtonObj;

    private string Typ;
    private string color;

    private Vector2 mousePos;
    private GameObject currentPen;

    public List<GameObject> Linien;

    public bool isDrawing = false;

    public GameObject switchPanel;
    public GameObject ColorPanel;

    public GameObject SizePanel;
    public Slider SizeSlider;

    public GameObject BG;
    void Start()
    {
       
        ColorPanel.SetActive(false);
        switchPanel.SetActive(false);
        SizePanel.SetActive(false);

        Typ = "Pen";
        color = "black";

        PenButton.onClick.AddListener(PenWechsel);
        EraserButton.onClick.AddListener(EraserWechsel);
        BrushButton.onClick.AddListener(BrushWechsel);

        DeleteButton.onClick.AddListener(deleteAllLines);
        switchButton.onClick.AddListener(openSwitchPanel);
        colorButton.onClick.AddListener(openColorPanel);
        sizeButton.onClick.AddListener(openSizePanel);

        blueButton.onClick.AddListener(changeToBlue);
        blackButton.onClick.AddListener(changeToBlack);
        redButton.onClick.AddListener(changeToRed);
        whiteButton.onClick.AddListener(changeToWhite);
        greenButton.onClick.AddListener(changeToGreen);
        FillButton.onClick.AddListener(Fill);
    }

    public void openSizePanel()
    {
        closeColorPanel();
        SizePanel.SetActive(true);
    }

    public void closeSizePanel()
    {
        SizePanel.SetActive(false);
    }
    public void openColorPanel()
    {
        ColorPanel.SetActive(true);
    }
    public void closeColorPanel()
    {
        ColorPanel.SetActive(false);
    }

    public void openSwitchPanel()
    {
        switchPanel.SetActive(true);
    }

    public void closeSwitchPanel()
    {
        switchPanel.SetActive(false);
    }

    public void Fill()
    {
        if (color == "white")
            BG.GetComponent<SpriteRenderer>().color = Color.white;
        if(color == "blue")
            BG.GetComponent<SpriteRenderer>().color = Color.blue;
        if(color == "red")
            BG.GetComponent<SpriteRenderer>().color = Color.red;
        if(color == "green")
            BG.GetComponent<SpriteRenderer>().color = Color.green;
        if(color == "black")
            BG.GetComponent<SpriteRenderer>().color = Color.black;

    }

    public void BrushWechsel()
    {
        Typ = "Brush";
        closeSwitchPanel();
        closeColorPanel();

    }
    public void EraserWechsel()
    {
        Typ = "Eraser";
        closeSwitchPanel();
        closeColorPanel();
    }
    public void PenWechsel()
    {
        Typ = "Pen";
        closeSwitchPanel();
        closeColorPanel();
    }



    void Update()
    {
       
        if (switchPanel.active == true)
        {
           
            SwitchButtonObj.SetActive(false);
        }
            

        else
        {
            
            SwitchButtonObj.SetActive(true);
        }
            

        if (Typ == "Brush")
        {
            BrushButton.interactable = false;
            PenButton.interactable = true;
            EraserButton.interactable = true;
        }

        if(Typ == "Pen")
        {
            BrushButton.interactable = true;
            PenButton.interactable = false;
            EraserButton.interactable = true;
        }

        if(Typ == "Eraser")
        {
            BrushButton.interactable = true;
            PenButton.interactable = true;
            EraserButton.interactable = false;
        }

        if (Input.GetMouseButtonDown(0))
            spawn();

        if (Input.GetMouseButtonUp(0))
                isDrawing = false;

        if (!isDrawing)
            return;


        if (isDrawing)
        {
            currentPen.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }

    }

    public void changeToBlack()
    {
        color = "black";
        closeColorPanel();
        closeSwitchPanel();
    }
    public void changeToWhite()
    {
        color = "white";
        closeColorPanel();
        closeSwitchPanel();
    }
    public void changeToRed()
    {
        color = "red";
        closeColorPanel();
        closeSwitchPanel();
    }
    public void changeToBlue()
    {
        color = "blue";
        closeColorPanel();
        closeSwitchPanel();
    }
    public void changeToGreen()
    {
        color = "green";
        closeColorPanel();
        closeSwitchPanel();
    }


    public void deleteAllLines()
    {
        foreach (GameObject Line in Linien)
            Destroy(Line);

        Linien.Clear();

        closeColorPanel();
        closeSwitchPanel();
    }

    public void spawn()
    {
        if(!SizePanel.active && !ColorPanel.active && !switchPanel.active)
        {
            if (Typ == null)
                return;

            if (Typ == "Eraser")
            {
                currentPen = Instantiate(Eraser, mousePos, Quaternion.identity);
                currentPen.GetComponent<TrailRenderer>().widthMultiplier = SizeSlider.value;
                Linien.Add(currentPen);
            }

            if (Typ == "Pen")
            {
                currentPen = Instantiate(Pen, mousePos, Quaternion.identity);
                currentPen.GetComponent<TrailRenderer>().widthMultiplier = SizeSlider.value;

                if (color == "black")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.black;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.black;
                }


                if (color == "red")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.red;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.red;
                }


                if (color == "green")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.green;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.green;
                }


                if (color == "white")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.white;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.white;
                }


                if (color == "blue")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.blue;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.blue;
                }


                Linien.Add(currentPen);
            }

            if (Typ == "Brush")
            {
                currentPen = Instantiate(Brush, mousePos, Quaternion.identity);
                currentPen.GetComponent<TrailRenderer>().widthMultiplier = SizeSlider.value;

                if (color == "black")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.black;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.black;
                }


                if (color == "red")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.red;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.red;
                }


                if (color == "green")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.green;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.green;
                }


                if (color == "white")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.white;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.white;
                }


                if (color == "blue")
                {
                    currentPen.GetComponent<TrailRenderer>().startColor = Color.blue;
                    currentPen.GetComponent<TrailRenderer>().endColor = Color.blue;
                }


                Linien.Add(currentPen);
            }



            isDrawing = true;
        }
        
    }

    
}
