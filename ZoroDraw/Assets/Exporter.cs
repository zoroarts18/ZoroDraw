using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Exporter : MonoBehaviour
{
    public bool takeScreenOnNextFrame = false;
    public string fileName;
    public Pencil p;
    public InputField FileInputF;

    private void Start()
    {
        FileInputF.text = fileName;
    }


    private void OnPostRender()
    {
        if (takeScreenOnNextFrame)
        {
            string galleryPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/Galery";
            takeScreenOnNextFrame = false;
            RenderTexture renderT = GetComponent<Camera>().targetTexture;
            Texture2D renderResult = new Texture2D(renderT.width, renderT.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderT.width, renderT.height);
            renderResult.ReadPixels(rect, 0, 0);
            byte[] bytes = renderResult.EncodeToPNG();
            File.WriteAllBytes(galleryPath + "/" + FileInputF.text +".png", bytes);
            RenderTexture.ReleaseTemporary(renderT);
            GetComponent<Camera>().targetTexture = null;
            
            p.RenderDone();
        }
    }
}
