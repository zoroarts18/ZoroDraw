using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.IO;
using UnityEditor;
public class Pencil : MonoBehaviour
{
    public Texture2D PenCursor, EraserCursor, BrushCursor;
    public Button currentLayerBtn;
    public List<Button> LayerBtns;

    public Image CurColorIndicator;
    public Image CurColorPickerIndicator;
    public Exporter expo;
    public Color FileBTNSelectedColor, FileBTNNormalColor;
    public GameObject Indicator;

    [Header("Buttons")]
    public Button closeAppButton;
    public Button PenButton;
    public Button EraserButton;
    public Button BrushButton;
    public Button FillBtn;
    public Button ClearBtn;
    public Button ExportBtn;
    public Button FinalExportBtn;
    public Button currcolorBtn;
    public Button StartSectionBtn;
    public Button[] ToolBtns;

    [Header("Brush-Properties")]
    public GameObject SizePanel;
    public Slider SizeSlider;
    public Slider OpacitySlider;


    public GameObject LayerBtn;
    public SpriteRenderer BGColor;
    public GameObject Pen;
    public GameObject Eraser;
    public GameObject Brush;
    private Vector2 mousePos;
    private GameObject currentPen;
    public GameObject ExportUI;

    private bool isDrawing = false;
    private bool canDraw = false;
    private bool shapeForming = false;
    public List<Vector2> currentShapeVert;
    private SpriteCreator currentShapeFormer;
    public GameObject ShapeFormer;
    public Button ShapeFormerButton;
    public LineRenderer ShapeFormerPreviewLine;
    private bool LineOn = false;
    private string Typ;

    public GameObject BGVert, BGHor;
    public GameObject BG;
    private Color currentColor;
    private Color currentFieldColor, currenPickerColor;

    //Swiping
    private Vector3 startPosSwipe, EndPosSwipe;
    
    [Header("Export")]
    public Button TransparencyToggle;
    private bool TransparencyOn = false;
    public InputField FileName;
    public Camera DrawCam, RenderCam, RenderCamVert, RenderCamHor;
    public GameObject[] DrawUI;

    [Header("File-Creation")]
    public GameObject FileCreatorUI;
    public InputField NewFileName;
    public Button VertBtn, HorBtn;
    private bool Vert;
    public Button CreateFileBtn;
    public Button CreateFileFinalBtn;
    public Transform FileBtnHolders;
    public GameObject FileBtn;
    private int NoumberOfFiles;
    public Button currentFileBtn;

    public Button SpawnLineBtn;
    public GameObject Line;
    private GameObject currentLine;

    private int OpenPages = 0;
    public GameObject StartUI;
    public List<Button> FileBtns;

    private string[] FileNames;
    private List<Sprite> files;
    private bool toolSizeable = false;
    public RectTransform RectColorPicker;
    public Texture2D ColorPickerTexture;
    public bool colorPickerColorChanging = false;
    public Button ColorPickerButton;
    public Button choosePickerColorBtn, chooseFieldColorBtn;
    private Texture2D currentCursor;
    public GameObject ImageVorschauHor, ImageVorschauVert;
    public GameObject RecentColorBtn;
    public Transform RecentColorHolder;
    //public List<string> FileList;
    //public List<Texture2D> s;
    public GameObject GaleryImg;
    public Transform GaleryHolder;

    public Text SizeValue, OpacityValue;
    public bool canDrawSomething = false;
    public Button DonateBtn;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Donate()
    {
        Application.OpenURL("https://paypal.me/ZoroArts?locale.x=de_DE");
    }

    public void LoadImages()
    {
        foreach (Transform item in GaleryHolder.transform)
        {
            GameObject.Destroy(item.gameObject);
        }

        string galleryPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Galery";

        if (!Directory.Exists(galleryPath)) Directory.CreateDirectory(galleryPath);

        foreach (var file in Directory.EnumerateFiles(galleryPath))
        {
            byte[] texData = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(texData);
            GameObject newGaleryItem = Instantiate(GaleryImg) as GameObject;
            newGaleryItem.GetComponent<RawImage>().texture = tex;
            Debug.Log(tex.width + "*" + tex.height);

            //Debug.Log(tex.format);
            newGaleryItem.transform.parent = GaleryHolder;
            if (tex.width == 1080)
            {
                newGaleryItem.transform.localScale = new Vector3(0.5f, 1.5f, 1);
                newGaleryItem.GetComponent<GaleryImgBtn>().Vert = true;

            }
            else if (tex.width == 1920)
            {
                newGaleryItem.transform.localScale = new Vector3(1f, 1, 1);
                newGaleryItem.GetComponent<GaleryImgBtn>().Vert = false;
            }
        }
    }
    void Start()
    {
        DonateBtn.onClick.AddListener(Donate);
        LoadImages();
        VertBtn.onClick.AddListener(OrientationVert);
        HorBtn.onClick.AddListener(OrientationHor);
        SizePanel.SetActive(false);
        currentColor = Color.black;
        PenButton.onClick.AddListener(PenSwitch);
        EraserButton.onClick.AddListener(EraserSwitch);
        BrushButton.onClick.AddListener(BrushSwitch);
        FillBtn.onClick.AddListener(Fill);
        ClearBtn.onClick.AddListener(Clear);
        closeAppButton.onClick.AddListener(closeApp);
        ExportBtn.onClick.AddListener(Export);
        FinalExportBtn.onClick.AddListener(TakeScreen);
        TransparencyToggle.onClick.AddListener(ToggleTransparency);
        if (currcolorBtn != null)
        {
            currcolorBtn.interactable = false;
            currcolorBtn.GetComponent<Outline>().effectColor = Color.red;
        }
        TransparencyOn = false;
        TransparencyToggle.GetComponentInChildren<Text>().text = "";
        CreateFileFinalBtn.onClick.AddListener(CreateNewFile);
        CreateFileBtn.onClick.AddListener(NewFile);
        SpawnLineBtn.onClick.AddListener(ActivateLine);
        StartSectionBtn.onClick.AddListener(openStartUI);
        currentFieldColor = Color.black;
        currenPickerColor = Color.black;
        currentColor = currentFieldColor;

        chooseFieldColorBtn.onClick.AddListener(chooseFieldColor);
        choosePickerColorBtn.onClick.AddListener(choosePickerColor);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        CurColorIndicator.GetComponent<Outline>().effectColor = Color.red;
    }

    public void choosePickerColor()
    {
        currentColor = currenPickerColor;
        CurColorIndicator.GetComponent<Outline>().effectColor = Color.black;
        CurColorPickerIndicator.GetComponent<Outline>().effectColor = Color.red;
    }
    public void chooseFieldColor()
    {
        currentColor = currentFieldColor;
        CurColorIndicator.GetComponent<Outline>().effectColor = Color.red;
        CurColorPickerIndicator.GetComponent<Outline>().effectColor = Color.black;
    }
    public void ActivateColorPicker()
    {
        if (!colorPickerColorChanging) colorPickerColorChanging = true;
        else
        {
            colorPickerColorChanging = false;
            currentColor = currenPickerColor;
            CurColorIndicator.GetComponent<Outline>().effectColor = Color.black;
            CurColorPickerIndicator.GetComponent<Outline>().effectColor = Color.red;

            //if(CurColorPickerIndicator.GetComponent<Outline>().effectColor == Color.red) currentColor = currenPickerColor;

            GameObject newRecentColorBtn = Instantiate(RecentColorBtn) as GameObject;
            newRecentColorBtn.GetComponent<Image>().color = currenPickerColor;
            newRecentColorBtn.transform.parent = RecentColorHolder;
        }
    }
    public void openStartUI()
    {
        LoadImages();
        canDrawSomething = false;
        LineOn = false;
        StartUI.SetActive(true);
        StartSectionBtn.GetComponent<Image>().color = FileBTNSelectedColor;
        currentFileBtn.GetComponent<Image>().color = FileBTNNormalColor;
    }
    public void ActivateLine()
    {
        toolSizeable = true;
        currentCursor = null;

        LineOn = true;
        foreach (var item in ToolBtns)
        {
            item.interactable = true;
        }
        SpawnLineBtn.interactable = false;
    }
    public void selectOtherFile(GameObject Btn)
    {
        canDrawSomething = true;
        LineOn = false;
        StartUI.SetActive(false);
        StartSectionBtn.GetComponent<Image>().color = FileBTNNormalColor;

        if (currentFileBtn != null)
        {
            foreach (var item in currentFileBtn.GetComponent<FileContent>().Lines) item.SetActive(false);
            currentFileBtn.GetComponent<Image>().color = FileBTNNormalColor;
        }

        currentFileBtn = Btn.GetComponent<Button>();
        currentFileBtn.GetComponent<Image>().color = FileBTNSelectedColor;
        if(currentFileBtn.GetComponent<FileContent>().Vert)
        {
            BGVert.SetActive(true);
            BGHor.SetActive(false);
            BG = BGVert;
            RenderCam = RenderCamVert;
        }
        else
        {
            BGHor.SetActive(true);
            BGVert.SetActive(false);
            BG = BGHor;
            RenderCam = RenderCamHor;
        }

        foreach (var item in currentFileBtn.GetComponent<FileContent>().Lines) item.SetActive(true);
    }

    public void NewFile()
    {
        Vert = false;
        HorBtn.interactable = false;
        LineOn = false;
        FileCreatorUI.SetActive(true);
    }

    public void OrientationVert()
    {
        Vert = true;
        HorBtn.interactable = true;
        VertBtn.interactable = false;
    }
    public void OrientationHor()
    {
        Vert = false;
        HorBtn.interactable = false;
        VertBtn.interactable = true;
    }

    public void DeletePage(Button DeleteButton)
    {
        LineOn = false;
        OpenPages--;
        foreach (var item in DeleteButton.GetComponent<FileContent>().Lines)
        {
            Destroy(item);
        }

        FileBtns.Remove(DeleteButton);

        if (OpenPages == 0)
        {
            StartSectionBtn.GetComponent<Image>().color = FileBTNSelectedColor;
            StartUI.SetActive(true);
        }

        else
        {
            selectOtherFile(FileBtns[OpenPages - 1].gameObject);
        }

        Destroy(DeleteButton.gameObject);
    }

    public void CreateNewFile()
    {
        HorBtn.interactable = true;
        VertBtn.interactable = true;

        canDrawSomething = true;

        if(currentFileBtn != null) currentFileBtn.GetComponent<Image>().color = FileBTNNormalColor;

        if (OpenPages == 0) StartUI.SetActive(false);

        if (Vert)
        {
            BGVert.SetActive(true);
            BGHor.SetActive(false);
            BG = BGVert;
        }
        else
        {
            BGHor.SetActive(true);
            BGVert.SetActive(false);
            BG = BGHor;
        }

        FileCreatorUI.SetActive(false);
        OpenPages++;
        if(currentFileBtn != null)
        {
            foreach (var item in currentFileBtn.GetComponent<FileContent>().Lines) item.SetActive(false);
            currentFileBtn.GetComponent<Image>().color = FileBTNNormalColor;
        }
        StartSectionBtn.GetComponent<Image>().color = FileBTNNormalColor;

        GameObject NewFileBtnInstantiate = Instantiate(FileBtn) as GameObject;
        NewFileBtnInstantiate.transform.parent = FileBtnHolders;
        NewFileBtnInstantiate.GetComponent<FileContent>().FileName = NewFileName.text;
        NewFileBtnInstantiate.gameObject.transform.localScale = new Vector3(1, 1, 1);
        currentFileBtn = NewFileBtnInstantiate.GetComponent<Button>();
        currentFileBtn.GetComponent<Image>().color = FileBTNSelectedColor;
        if (Vert)
        {
            NewFileBtnInstantiate.GetComponent<FileContent>().Vert = true;
            RenderCam = RenderCamVert;
        }
        else
        {
            NewFileBtnInstantiate.GetComponent<FileContent>().Vert = false;
            RenderCam = RenderCamHor;
        }

        FileBtns.Add(currentFileBtn);
    }

    #region Export
    public void TakeScreen()
    {
        if(currentFileBtn.GetComponent<FileContent>().Vert == true)
        {
            //Screen.SetResolution(1080, 1920, true);
            RenderCam = RenderCamVert;
            RenderCam.targetTexture = RenderTexture.GetTemporary(1080,1920, 16);
        }
        else
        {
            //Screen.SetResolution(1920, 1080, true);
            RenderCam = RenderCamHor;
            RenderCam.targetTexture = RenderTexture.GetTemporary(1920,1080, 16);
        }

        if (TransparencyOn) BG.SetActive(false);
        else BG.SetActive(true);

        RenderCam.GetComponent<Exporter>().takeScreenOnNextFrame = true;
    }
    public void Export()
    {
        if (currentFileBtn.GetComponent<FileContent>().Vert) RenderCam = RenderCamVert;
        else RenderCam = RenderCamHor;

        expo = RenderCam.GetComponent<Exporter>();

        FileName.text = currentFileBtn.GetComponent<FileContent>().FileName;
        expo.fileName = currentFileBtn.GetComponent<FileContent>().FileName;
        

        canDrawSomething = false;
        LineOn = false;
        ExportUI.SetActive(true);
        canDraw = false;

        DrawCam.gameObject.SetActive(false);
        RenderCam.gameObject.SetActive(true);

        foreach (var item in DrawUI) item.SetActive(false);
    }
    public void ToggleTransparency()
    {
        if (TransparencyOn)
        {
            TransparencyOn = false;
            TransparencyToggle.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            TransparencyOn = true;
            TransparencyToggle.GetComponentInChildren<Text>().text = "X";
        }
    }
    public void RenderDone()
    {
        canDrawSomething = true;
        //Screen.SetResolution(1920, 1080, true);
        ExportUI.SetActive(false);
        DrawCam.gameObject.SetActive(true);
        RenderCam.gameObject.SetActive(false);
        foreach (var item in DrawUI) item.SetActive(true);
        canDraw = true;
        BG.SetActive(true);
    }
    #endregion

    public void Fill()
    {
        if(canDrawSomething)
        {
            currentCursor = null;

            toolSizeable = false;
            LineOn = false;
            BG.GetComponent<SpriteRenderer>().color = currentColor;
            foreach (var item in currentFileBtn.GetComponent<FileContent>().Erasers)
            {
                item.GetComponent<TrailRenderer>().startColor = currentColor;
                item.GetComponent<TrailRenderer>().endColor = currentColor;
            }
        }
        
    }
    public void Clear()
    {
        if(canDrawSomething)
        {
            currentCursor = null;

            toolSizeable = false;
            LineOn = false;
            BG.GetComponent<SpriteRenderer>().color = Color.white;

            currentFileBtn.GetComponent<FileContent>().Erasers.Clear();
            foreach (var item in currentFileBtn.GetComponent<FileContent>().Lines) Destroy(item);
            currentFileBtn.GetComponent<FileContent>().Lines.Clear();
        }
    }
    public void closeApp()
    {
        Application.Quit();
    }
    public void openSizePanel()
    {
        if(toolSizeable)
        {
            canDrawSomething = false;
            LineOn = false;
            SizePanel.SetActive(true);
            SizePanel.transform.position = Input.mousePosition;
        }
    }
    public void closeSizePanel()
    {
        canDrawSomething = true;
        SizePanel.SetActive(false);
    }
    public void BrushSwitch()
    {
        if(canDrawSomething)
        {
            currentCursor = BrushCursor;

            toolSizeable = true;
            LineOn = false;
            Typ = "Brush";
            foreach (var item in ToolBtns) item.interactable = true;
            BrushButton.interactable = false;
        }
        
    }
    public void EraserSwitch()
    {
        if(canDrawSomething)
        {
            currentCursor = EraserCursor;

            toolSizeable = true;
            LineOn = false;
            Typ = "Eraser";
            foreach (var item in ToolBtns) item.interactable = true;
            EraserButton.interactable = false;
        }
        
    }
    public void PenSwitch()
    {
        if(canDrawSomething)
        {
            currentCursor = PenCursor;

            toolSizeable = true;
            LineOn = false;
            Typ = "Pen";
            foreach (var item in ToolBtns) item.interactable = true;
            PenButton.interactable = false;
        }
        
    }
    public void SetColor(Button ColorBtn)
    {
        if(canDrawSomething)
        {
            GameObject newRecentColorBtn = Instantiate(RecentColorBtn) as GameObject;
            newRecentColorBtn.GetComponent<Image>().color = ColorBtn.GetComponent<Image>().color;
            newRecentColorBtn.transform.parent = RecentColorHolder;

            if (currcolorBtn != null)
            {
                currcolorBtn.interactable = true;
                currcolorBtn.GetComponent<Outline>().effectColor = Color.white;
            }

            currcolorBtn = ColorBtn;
            if (currcolorBtn != null)
            {
                currcolorBtn.interactable = false;
                currcolorBtn.GetComponent<Outline>().effectColor = Color.red;
            }

            currentFieldColor = ColorBtn.GetComponent<Image>().color;

            if (CurColorIndicator.GetComponent<Outline>().effectColor == Color.red) currentColor = currentFieldColor;
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && canDrawSomething)
        {
            currentColor = currenPickerColor;
            CurColorIndicator.GetComponent<Outline>().effectColor = Color.black;
            CurColorPickerIndicator.GetComponent<Outline>().effectColor = Color.red;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && canDrawSomething)
        {
            currentColor = currentFieldColor;
            CurColorIndicator.GetComponent<Outline>().effectColor = Color.red;
            CurColorPickerIndicator.GetComponent<Outline>().effectColor = Color.black;
        }

        if (FileBtns.Count >= 8) CreateFileBtn.interactable = false;
        else CreateFileBtn.interactable = true;

        //ColorPalette:
        if (colorPickerColorChanging)
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RectColorPicker, Input.mousePosition, null, out delta);
            float width = RectColorPicker.rect.width;
            float height = RectColorPicker.rect.height;
            delta += new Vector2(width * .5f, height * .5f);
            float x = Mathf.Clamp(delta.x / width, 0, 1);
            float y = Mathf.Clamp(delta.y / height, 0, 1);

            int texX = Mathf.RoundToInt(x * ColorPickerTexture.width);
            int texY = Mathf.RoundToInt(y * ColorPickerTexture.height);
            Debug.Log(texX);
            Debug.Log(texY);
            currenPickerColor = ColorPickerTexture.GetPixel(texX, texY);
            if (texX < 1020 && texX > 8 && texY < 1011 && texY > 4) ColorPickerButton.transform.position = Input.mousePosition;
        }
        
        //Line:
        if (LineOn && currentLine == null && Input.GetMouseButtonDown(0) && canDrawSomething)
        {
            Debug.Log(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1));
            GameObject newLine = Instantiate(Line, Vector3.zero, Quaternion.identity); 
            newLine.GetComponent<LineRenderer>().positionCount = 1;
            newLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1));
            newLine.GetComponent<LineRenderer>().startColor = currentColor;
            newLine.GetComponent<LineRenderer>().endColor = currentColor;
            newLine.GetComponent<LineRenderer>().widthMultiplier = SizeSlider.value / 4f;
            currentFileBtn.GetComponent<FileContent>().Lines.Add(newLine);
            
            currentLine = newLine;
        }
        if (LineOn && currentLine != null && Input.GetMouseButton(0) && canDrawSomething)
        {
            currentLine.GetComponent<LineRenderer>().positionCount = 2;
            currentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1));
        }
        if (LineOn && Input.GetMouseButtonUp(0) && currentLine.GetComponent<LineRenderer>().positionCount == 2 && canDrawSomething) currentLine = null;

        if (Input.GetMouseButton(0) && canDrawSomething) canDraw = true;

        //Moving around on Canvas:
        if (Input.GetKey(KeyCode.V)) canDraw = false;
        if (Input.GetKey(KeyCode.V) && Input.GetMouseButtonDown(0)) startPosSwipe = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.V) && Input.GetMouseButton(0))
        {
            EndPosSwipe = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = startPosSwipe - EndPosSwipe;
            Camera.main.transform.position += dir;
        }

        //"spawn" instantiates the Trail Renderer for Drawing (A Trail Renderer is following the Mouse now)
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && canDraw && canDrawSomething) spawn();
        if (Input.GetMouseButtonUp(0)) isDrawing = false;
        
        //Zooming:
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetAxisRaw("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize < 15) Camera.main.orthographicSize++;
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetAxisRaw("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize > 1) Camera.main.orthographicSize--;

        //Tool Switch Shortcuts:
        if (Input.GetKeyDown(KeyCode.E) && canDrawSomething) EraserSwitch();
        if (Input.GetKeyDown(KeyCode.B) && canDrawSomething) BrushSwitch();
        if(Input.GetKeyDown(KeyCode.P) && canDrawSomething) PenSwitch();
        if (Input.GetKeyDown(KeyCode.F) && canDrawSomething) Fill();
        if (Input.GetKeyDown(KeyCode.L) && canDrawSomething) ActivateLine();

        if (SizePanel.activeInHierarchy && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            canDrawSomething = true;
            SizePanel.SetActive(false);
        }
        if (Input.GetMouseButtonDown(1)) openSizePanel();  
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && currentFileBtn.GetComponent<FileContent>().Lines.Count > 0 && canDrawSomething)
        {
            Destroy(currentFileBtn.GetComponent<FileContent>().Lines[currentFileBtn.GetComponent<FileContent>().Lines.Count - 1].gameObject);
            currentFileBtn.GetComponent<FileContent>().Lines.RemoveAt(currentFileBtn.GetComponent<FileContent>().Lines.Count - 1);
        }
        if (isDrawing && canDraw) currentPen.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 9);
        CurColorIndicator.color = currentFieldColor;
        CurColorPickerIndicator.color = currenPickerColor;

        //Change Cursor while hovering over UI:
        if (EventSystem.current.IsPointerOverGameObject()) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        else Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);

        //Brush Values:
        var SizeVal = SizeSlider.value;
        var OpVal = OpacitySlider.value;
        SizeValue.text = SizeVal.ToString("F2");
        OpacityValue.text = OpVal.ToString("F2");
    }
    public void deleteAllLines()
    {
        foreach (GameObject Line in currentFileBtn.GetComponent<FileContent>().Lines) Destroy(Line);
        currentFileBtn.GetComponent<FileContent>().Lines.Clear();
    }
    public void spawn()
    {
        if(canDraw && !shapeForming && !LineOn && canDrawSomething)
        {
            if (Typ == "Eraser")
            {
                currentPen = Instantiate(Eraser, (Vector3)mousePos + new Vector3(0,0, -1), Quaternion.identity);
                currentPen.GetComponent<TrailRenderer>().widthMultiplier = SizeSlider.value;
                currentFileBtn.GetComponent<FileContent>().Lines.Add(currentPen);
                currentFileBtn.GetComponent<FileContent>().Erasers.Add(currentPen);
                currentPen.GetComponent<TrailRenderer>().startColor = BG.GetComponent<SpriteRenderer>().color;
                currentPen.GetComponent<TrailRenderer>().endColor = BG.GetComponent<SpriteRenderer>().color;
                isDrawing = true;
            }

            if (Typ == "Pen")
            {
                currentPen = Instantiate(Pen, (Vector3)mousePos + new Vector3(0, 0, -1), Quaternion.identity);
                currentPen.GetComponent<TrailRenderer>().widthMultiplier = SizeSlider.value;
                currentPen.GetComponent<TrailRenderer>().startColor = new Color(currentColor.r, currentColor.g, currentColor.b, OpacitySlider.value);
                currentPen.GetComponent<TrailRenderer>().endColor = new Color(currentColor.r, currentColor.g, currentColor.b, OpacitySlider.value);
                currentFileBtn.GetComponent<FileContent>().Lines.Add(currentPen);
                isDrawing = true;
            }

            if (Typ == "Brush")
            {
                currentPen = Instantiate(Brush, (Vector3)mousePos + new Vector3(0, 0, -1), Quaternion.identity);
                currentPen.GetComponent<TrailRenderer>().widthMultiplier = SizeSlider.value;
                currentPen.GetComponent<TrailRenderer>().startColor = new Color(currentColor.r, currentColor.g, currentColor.b, OpacitySlider.value);
                currentPen.GetComponent<TrailRenderer>().endColor = new Color(currentColor.r, currentColor.g, currentColor.b, OpacitySlider.value);
                currentFileBtn.GetComponent<FileContent>().Lines.Add(currentPen);
                isDrawing = true;
            }
        }  
    }
}
