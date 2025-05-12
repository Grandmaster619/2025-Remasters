using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFunctionDrawer : MonoBehaviour {


    public Camera camera;
    public MousePathData mpd1 = new MousePathData();
    public bool editMode = true;
	// Use this for initialization
	void Start () {
        if(!editMode)
        {
            mpd1 = JsonUtility.FromJson<MousePathData>(System.IO.File.ReadAllText(Application.dataPath + "mouseData.txt"));
        }
	}

    void OnDestroy()
    {
        if (editMode)
        {
            System.IO.File.WriteAllText(Application.dataPath + "mouseData.txt", JsonUtility.ToJson(mpd1));
        }
    }
	

    void Update()
    {
        mpd1.DrawMousePathData();
    }


    

	
	void FixedUpdate () {
		if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Floor");
            Debug.Log(LayerMask.NameToLayer("Floor"));
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                mpd1.points.Add(new Vector3(hit.point.x, 0, hit.point.z));
                Debug.Log(hit.point);
            }
        }
	}

    public class MousePathData
    {
        public List<Vector3> points = new List<Vector3>();
        public void DrawMousePathData()
        {
            for (int i = 0; i < points.Count - 1; i += 1)
            {
                Debug.DrawLine(points[i], points[i + 1], Color.green);
            }
        }
    }

}
