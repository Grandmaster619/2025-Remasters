using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    private const float MIN_BRIGHTNESS = 0.05f;

    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Text brightnessText;
    [SerializeField] private PostProcessProfile brightness;
    private AutoExposure exposure;

    void Start()
    {
        brightness.TryGetSettings(out exposure);

        brightnessSlider.value = exposure.keyValue.value;
        brightnessText.text = "" + brightnessSlider.value;

        brightnessSlider.onValueChanged.AddListener((f) => AdjustValue(f));
    }

    private void AdjustValue(float value)
    {
        exposure.keyValue.value = value > MIN_BRIGHTNESS ? value : MIN_BRIGHTNESS;
        brightnessText.text = "" + value;
    }

    public float GetBrightness() { return exposure.keyValue.value; }
}
