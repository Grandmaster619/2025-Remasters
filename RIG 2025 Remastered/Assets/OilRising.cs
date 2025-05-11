using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OilRising : MonoBehaviour
{
    [SerializeField] private float MaxHeight = 1.8f;
    [SerializeField] private float MinHeight = 0.3f;
    [SerializeField] private float ToggleTime = 6f;
    [SerializeField] private float Speed = 2f;
    private bool DirectionToggle = true;
    private float CurrentTime = 0f;

    private Vector3 HighPosition;
    private Vector3 LowPosition;

    private void Start()
    {
        HighPosition = new Vector3(transform.position.x, MaxHeight, transform.position.z);
        LowPosition = new Vector3(transform.position.x, MinHeight, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTime < 0f)
        {
            DirectionToggle = !DirectionToggle;
            CurrentTime = ToggleTime;
        }

        float step = Speed * Time.deltaTime;
        if (DirectionToggle)
        {
            //Debug.Log("Traveling Up");
            transform.position = Vector3.MoveTowards(transform.position, HighPosition, step);
        }
        else
        {
            //Debug.Log("Traveling Down");
            transform.position = Vector3.MoveTowards(transform.position, LowPosition, step);
        }

        CurrentTime -= Time.deltaTime;
    }
}
