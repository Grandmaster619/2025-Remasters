using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PiecewiseHandler : MonoBehaviour {

    public delegate Vector3 MathFunction(float time);

    public delegate void SetData();

    public int data;

    // Use this for initialization
    void Start()
    {
        DrawLine dl = new DrawLine(new Vector3(0,0,0), new Vector3(1,0,1));
        

    }

    void TestParams()
    {
        FormulaDraw fd = FindObjectOfType<FormulaDraw>();
        Debug.Log(fd.GetType());
        Debug.Log(typeof(ClassDataTest).GetConstructors()[0].GetParameters()[2].ParameterType);
        foreach (MemberInfo minfo in fd.GetType().GetMembers())
        {
            Debug.Log("Member:: " + minfo.ToString());

        }
        foreach (PropertyInfo prop in fd.GetType().GetProperties())
        {
            Debug.Log("Property:: " + prop.ToString());

        }
        foreach (MethodInfo mInfo in fd.GetType().GetMethods())
        {
            Debug.Log("Method:: " + mInfo.ToString());
        }
    }



	// Update is called once per frame
	void Update () {
		
	}
}
