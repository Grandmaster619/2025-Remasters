using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine {

    public Vector3 startPoint, endPoint;

    public DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        this.endPoint = endPoint;
        this.startPoint = startPoint;
    }

    public void DrawRedLine()
    {
        Debug.DrawLine(startPoint, endPoint);
    }

    public void Function(float t)
    {

    }
    
}
