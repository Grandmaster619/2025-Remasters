using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaDraw : MonoBehaviour {

    public int x_Formula_Index = 0;
    public int y_Formula_Index = 0;

    private delegate Vector3 MathFunction(float time);

    // Use this for initialization
    void Start () {
        Vector3 test = new Vector3(7, 9, 7);
        
	}
	
	// Update is called once per frame
	void Update () {
        DrawFunctionDashed(Function1, 1 / 200f);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private Vector3 Function1(float time)
    {
        //0<=t<=20
        float t = time * 10;
        float t2 = time * 10;

        float[] x_Formulas = {((t) * (t - 10) * (t - 5)) / 5,
                            ((t) * (t - 10) * (t - 5) * (t - 2) * (t - 7)) / 50,
                            ((t) * (t - 10) * (t - 4) * (t - 6)) / 25,
                            ((t) * (t - 10) * (t - 7)) / 50,
                            ((t) * (t - 10) * (t - 5) * (t - 3) * (t - 8)) / 60,
                            (Mathf.Pow(2, -t) * t * (t - 10) * (t - 3))};

        float[] y_Formulas = { Mathf.Abs(((t2) * (t2 - 10) * (t2 - 5) * (t2 - 2) * (t2 - 8)) / 25),
                                Mathf.Abs(((t2) * (t2 - 10) * (t2 - 2) * (t2 - 8)) / 25),
                                Mathf.Abs(((t2) * (t2 - 10) * (t2 - 3) * (t2 - 7)) / 25),
                                Mathf.Abs(((t2) * (t2 - 10) * (t2 - 3) * (t2 - 8)) / 25),
                                Mathf.Abs(((t2) * (t2 - 10) * (t2 - 2) * (t2 - 7)) / 25),
                                Mathf.Abs(Mathf.Pow(2, -t) * t * (t - 10) * (t - 3))};

        float x = x_Formulas[x_Formula_Index];

        float y = y_Formulas[y_Formula_Index];

        return new Vector3(x, 0, y);
    }

    private void DrawFunctionDashed(MathFunction function_of_t, float interval)
    {
        //Find all the points to find the t value at.
        float time_x = 0f;
        List<float> t_values = new List<float>();
        t_values.Add(time_x);
        while (time_x < 1f)
        {
            t_values.Add(time_x += interval);
        }
        t_values.Add(1f);
        List<Vector3> functionPoints = new List<Vector3>();
        //Calculate the function points
        foreach (float t_value in t_values)
        {
            functionPoints.Add(function_of_t(t_value));
        }
        for (int i = 0; i < functionPoints.Count - 1; i += 2)
        {
            Debug.DrawLine(functionPoints[i], functionPoints[i + 1], Color.green);
        }
    }

    //private Color HeatFunction(float input, float min, float max)
    //{
    //    float percentage = input/(max - min);



    //}


}
