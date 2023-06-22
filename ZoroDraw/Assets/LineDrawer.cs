using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject LinePenPref;
    public GameObject currLine;
    public bool drawn = false;

    void Update()
    {
        if (currLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currLine.GetComponent<LinePen>().AddPoint(mousePos);
        }

        if (Input.GetMouseButtonDown(0) && !drawn)
        {
            currLine = Instantiate(LinePenPref, this.transform.position, Quaternion.identity);
            drawn = true;
        }
       
        if (Input.GetMouseButtonUp(0))
        {
            if(currLine != null) currLine.GetComponent<LinePen>().ready = true;
            currLine = null;
        }
    }

    public void EraseLine(GameObject Line, Vector2 point, List<Vector2> points)
    {
        EdgeCollider2D edgeC = Line.GetComponent<EdgeCollider2D>();
        Line.GetComponent<LinePen>().points.Remove(point);
    }
}
