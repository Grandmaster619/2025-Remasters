using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;

        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirFromAngleBreadth(fov.viewAngleBreadth / 2, false);
        Vector3 viewAngleB = fov.DirFromAngleBreadth(-fov.viewAngleBreadth / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        Handles.DrawWireArc(fov.transform.position, Vector3.right, Vector3.up, 360, fov.viewRadius);
        Vector3 viewAngleC = fov.DirFromAngleDepth(fov.viewAngleDepth / 2, false);
        Vector3 viewAngleD = fov.DirFromAngleDepth(-fov.viewAngleDepth / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleC * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleD * fov.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}

#endif

