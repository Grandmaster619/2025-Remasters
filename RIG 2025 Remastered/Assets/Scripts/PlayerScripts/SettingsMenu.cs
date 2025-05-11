using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Text senseText;

    void Start()
    {
        mouseSensitivitySlider.value = Settings.GetInstance().GetMouseSensitivity();
        senseText.text = "" + mouseSensitivitySlider.value;

        mouseSensitivitySlider.onValueChanged.AddListener((f) => ChangeMouseSensitivity(f));

        gameObject.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        gameObject.SetActive(false);
    }

    private void ChangeMouseSensitivity(float value)
    {
        Settings.GetInstance().SetMouseSensitivity(value);
        senseText.text = "" + mouseSensitivitySlider.value;
    }
}
