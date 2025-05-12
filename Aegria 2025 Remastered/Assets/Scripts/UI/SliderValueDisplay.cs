using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{

    [SerializeField]
    private Slider parentSlider;
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Text>().text = Mathf.Round(parentSlider.value).ToString();
	}
}
