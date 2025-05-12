using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PiecewiseHandler))]
public class TestInspector : Editor {

        public override void OnInspectorGUI()
        {
            PiecewiseHandler myTarget = (PiecewiseHandler)target;

            myTarget.data = EditorGUILayout.IntField("Data", myTarget.data);
            EditorGUILayout.LabelField("Level", "54");
        }
}

#endif
