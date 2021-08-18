using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePen : MonoBehaviour
{
    LineRenderer Line;
    EdgeCollider2D edgeColl;
    public bool ready = false;

    public List<Vector2> points = new List<Vector2>();
    public int pointsCount = 0;
    float pointsMinDistance = .05f;
    float width = 0.2f;
    bool drawing = false;
    public int PointToRemove = 0;
    public bool erasing = false;

    void Start()
    {
        Line = GetComponent<LineRenderer>();
        edgeColl = GetComponent<EdgeCollider2D>();

        SetLineWidth(width);
    }

    public Vector3 GetLastPoint()
    {
        return (Vector3)Line.GetPosition(pointsCount - 1);
    }

    public void AddPoint(Vector2 point)
    {
        if (pointsCount >= 1 && Vector2.Distance(point, GetLastPoint()) < pointsMinDistance) return;

        points.Add(point);
        pointsCount++;
        Line.positionCount = pointsCount;
        Line.SetPosition(pointsCount - 1, point);
        if(pointsCount > 1)
        {
           edgeColl.points = points.ToArray() ;
        }
    }


    private void OnMouseEnter()
    {
        if (erasing)
        {
            CheckWhichPointIsNearestToMouse();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) erasing = true;
    }

    public void CheckWhichPointIsNearestToMouse()
    {
        Vector2 nearPoint;
        float Distance = 100;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < points.Count; i++)
        {
            if(Vector2.Distance(points[i], Camera.main.ScreenToWorldPoint(Input.mousePosition)) < Distance)
            {
                Distance = Vector2.Distance(points[i], mousePos);
                nearPoint = points[i];
                PointToRemove = i;
            }
        }



        //points.Remove(points[PointToRemove]);
        Line.positionCount = PointToRemove;
        edgeColl.Reset();

        for (int i = PointToRemove; i < Line.positionCount; i++)
        {
            points.RemoveAt(i);
        }
        edgeColl.points = points.ToArray();


        //Line.positionCount = 0;
        //
        foreach (var item in points)
        {
            Line.positionCount++;
            Line.SetPosition(Line.positionCount -1, item);
        }
        //
    }

    public void SetPointMinDistance(float distance)
    {
        pointsMinDistance = distance;
    }

    public void SetLineWidth(float width)
    {
        Line.startWidth = width;
        Line.endWidth = width;

        edgeColl.edgeRadius = width/ 2;
    }
}
